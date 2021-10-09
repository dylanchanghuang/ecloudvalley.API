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
                        options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore; //�t��null����쪺����
                        //options.SerializerSettings.Formatting = Formatting.Indented; //Json�^�Ǯ榡���۰��Y��
                    });

            #region �Nappsetttings.json�����bind�� ConfigManager => �A�h���R�A��ConfigProvider�A���򳣨ϥΥ�
            //1. �Nappsetttings.json�����bind�� ConfigManager
            var configManager = new ConfigManager();
            Configuration.Bind(configManager);
            //2. �A�h���R�A��ConfigProvider�A���򳣨ϥΥ� (ConfigProvider)
            ConfigProvider.Configure(configManager);
            #endregion

            //�[�J API����
            services.AddApiVersioning(options => {
                // �bresponse header��ܸ�api�䴩������
                options.ReportApiVersions = true;
                // �O�_�b�����w�����ɨϥιw�]����
                options.AssumeDefaultVersionWhenUnspecified = true;
                // �w�]��API����
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

            //�ϥ� Swashbuckle �M�󲣥�API���
            services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
            services.AddSwaggerGen(options => options.OperationFilter<SwaggerDefaultValues>());

            #region ������ҡBAutoMapper�MMediatR �P Service
            //1. ���h��������ҡBAutoMapper�MMediatR
            services.AddDomain();

            //2. Service �`�J
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
                    //API�¤��榡
                    app.UseReDoc(config =>
                    {
                        config.DocumentTitle = $"�춳��API {description.GroupName.ToUpperInvariant()}";
                        // �o�̪� RoutePrefix �Ψӳ]�w ReDoc UI ������ (���}���|)
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
