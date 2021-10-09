using ecloudvalley.Application.Models.Bill;
using ecloudvalley.Infrastructure.SharedKernel.Interfaces.Services;
using ecloudvalley.Infrastructure.SharedKernel.Models.Api;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ecloudvalley.Application.Service.Bill
{
    public interface IBillService : IService
    {
        /// <summary>
        /// 取得指定使用者帳號Id的未混和成本
        /// </summary>
        /// <param name="model">AccountModel</param>
        /// <returns></returns>
        Task<ApiResultModel> GetUnblendedCostAsync(AccountModel model);

        /// <summary>
        /// 取得指定使用者帳號Id的使用金額
        /// </summary>
        /// <param name="model">AccountModel</param>
        /// <returns></returns>
        Task<ApiResultModel> GetUsageAmountAsync(AccountModel model);
    }
}
