using ecloudvalley.Application;
using ecloudvalley.Domain;
using ecloudvalley.Infrastructure.Swagger;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ecloudvalley.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers()
                    .AddNewtonsoftJson(options =>
                    {
                        //options.SerializerSettings.ContractResolver = new DefaultContractResolver(); //Pascal Case
                        options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore; //含有null值欄位的忽略
                        //options.SerializerSettings.Formatting = Formatting.Indented; //Json回傳格式的自動縮排
                    });

            #region 將appsetttings.json的資料bind到 ConfigManager => 再搬到靜態的ConfigProvider，後續都使用它
            //1. 將appsetttings.json的資料bind到 ConfigManager
            var configManager = new ConfigManager();
            Configuration.Bind(configManager);
            //2. 再搬到靜態的ConfigProvider，後續都使用它 (ConfigProvider)
            ConfigProvider.Configure(configManager);
            #endregion

            //加入 API版本
            services.AddApiVersioning(options => {
                // 在response header顯示該api支援的版本
                options.ReportApiVersions = true;
                // 是否在未指定版本時使用預設版本
                options.AssumeDefaultVersionWhenUnspecified = true;
                // 預設的API版本
                options.DefaultApiVersion = new ApiVersion(1, 0);
            });

            services.AddVersionedApiExplorer(options =>
            {
                // add the versioned api explorer, which also adds IApiVersionDescriptionProvider service
                // note: the specified format code will format the version as "'v'major[.minor][-status]"
                options.GroupNameFormat = "'v'VVV";

                // note: this option is only necessary when versioning by url segment. the SubstitutionFormat
                // can also be used to control the format of the API version in route templates
                options.SubstituteApiVersionInUrl = true;
            });

            //使用 Swashbuckle 套件產生API文件
            services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
            services.AddSwaggerGen(options => options.OperationFilter<SwaggerDefaultValues>());

            #region 資料驗證、AutoMapper和MediatR 與 Service
            //1. 領域層的資料驗證、AutoMapper和MediatR
            services.AddDomain();

            //2. Service 注入
            services.AddServices();
            #endregion
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IApiVersionDescriptionProvider provider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                app.UseSwagger();
                app.UseSwaggerUI(options =>
                {
                    // build a swagger endpoint for each discovered API version
                    foreach (var description in provider.ApiVersionDescriptions)
                    {
                        options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
                    }
                });

                foreach (var description in provider.ApiVersionDescriptions)
                {
                    //API純文件格式
                    app.UseReDoc(config =>
                    {
                        config.DocumentTitle = $"伊雲谷API {description.GroupName.ToUpperInvariant()}";
                        // 這裡的 RoutePrefix 用來設定 ReDoc UI 的路由 (網址路徑)
                        config.RoutePrefix = $"redoc/{description.GroupName}";
                        config.SpecUrl = $"/swagger/{description.GroupName}/swagger.json";
                    });
                }
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
