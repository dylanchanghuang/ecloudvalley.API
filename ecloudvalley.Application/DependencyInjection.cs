using ecloudvalley.Application.Service.Bill;
using ecloudvalley.Infrastructure.SharedKernel.Interfaces.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace ecloudvalley.Application
{
    public static class DependencyInjection
    {
        /// <summary>
        /// Service 注入
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddServices(this IServiceCollection services)
        {

            #region 動態注入Service的介面
            // 上方的 Assembly資料要注入後, 再注入 Service 才不會發生問題
            // i assume your service interfaces inherit from IService
            Assembly ass = typeof(IBillService).GetTypeInfo().Assembly;

            // get all concrete types which implements IService
            var allServices = ass.GetTypes().Where(t =>
                t.GetTypeInfo().IsClass &&
                !t.GetTypeInfo().IsAbstract &&
                typeof(IService).IsAssignableFrom(t));

            foreach (var type in allServices)
            {
                var allInterfaces = type.GetInterfaces();
                var mainInterfaces = allInterfaces.Except
                        (allInterfaces.SelectMany(t => t.GetInterfaces()));
                foreach (var itype in mainInterfaces)
                {
                    if (allServices.Any(x => !x.Equals(type) && itype.IsAssignableFrom(x)))
                    {
                        throw new Exception("The " + itype.Name + " type has more than one implementations, please change your filter");
                    }
                    services.AddTransient(itype, type);
                }
            }
            #endregion

            return services;
        }
    }
}
