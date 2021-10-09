using ecloudvalley.Domain.Common.Behaviours;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace ecloudvalley.Domain
{
    public static class DependencyInjection
    {
        /// <summary>
        /// 資料驗證、AutoMapper和MediatR
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddDomain(this IServiceCollection services)
        {
            //將AutoMapper映射配置所在的程式名稱註冊
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            //註冊MediatR
            services.AddMediatR(Assembly.GetExecutingAssembly());
            //將驗證新增到MediatR
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));

            return services;
        }
    }
}
