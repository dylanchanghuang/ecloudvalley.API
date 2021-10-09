using ecloudvalley.Application.Models.Bill;
using ecloudvalley.Domain.Bill.Dtos;
using ecloudvalley.Domain.Bill.Queries;
using ecloudvalley.Infrastructure.SharedKernel.Models.Api;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ecloudvalley.Application.Service.Bill
{
    public class BillService : IBillService
    {
        private bool disposedValue;
        private readonly IMediator _mediator;
        private readonly ILogger<BillService> _logger;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: 處置受控狀態 (受控物件)
                }

                // TODO: 釋出非受控資源 (非受控物件) 並覆寫完成項
                // TODO: 將大型欄位設為 Null
                disposedValue = true;
            }
        }

        // // TODO: 僅有當 'Dispose(bool disposing)' 具有會釋出非受控資源的程式碼時，才覆寫完成項
        // ~BillService()
        // {
        //     // 請勿變更此程式碼。請將清除程式碼放入 'Dispose(bool disposing)' 方法
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // 請勿變更此程式碼。請將清除程式碼放入 'Dispose(bool disposing)' 方法
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        public BillService(IMediator mediator, ILogger<BillService> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        /// <summary>
        /// 取得指定使用者帳號Id的未混和成本
        /// </summary>
        /// <param name="model">AccountModel</param>
        /// <returns></returns>
        public async Task<ApiResultModel> GetUnblendedCostAsync(AccountModel model)
        {
            GetUnblendedCostQuery query = new GetUnblendedCostQuery();
            query.UsageAccountId = model.UsageAccountId;
            query.CurrentPage = model.CurrentPage <= 0 ? 1 : model.CurrentPage;
            query.PageSize = model.PageSize <= 0 ? 10 : model.PageSize;

            IDictionary<string, decimal> result = await _mediator.Send(query);
            //回傳
            ApiResultModel apiResult = new ApiResultModel();
            if (result != null)
            {
                if (result.Count > 0)
                {
                    apiResult.Code = 0;
                    apiResult.Message = string.Empty;
                    apiResult.Data = result;
                }
                else
                {
                    apiResult.Code = 1;
                    apiResult.Message = "此帳號已無更多未混和成本資料!!";
                    apiResult.Data = new object();
                }
            }
            else
            {
                apiResult.Code = 404;
                apiResult.Message = "使用者帳號Id不存在!!";
                apiResult.Data = new object();
            }

            return apiResult;
        }

        /// <summary>
        /// 取得指定使用者帳號Id的使用金額
        /// </summary>
        /// <param name="model">AccountModel</param>
        /// <returns></returns>
        public async Task<ApiResultModel> GetUsageAmountAsync(AccountModel model)
        {
            GetUsageAmountQuery query = new GetUsageAmountQuery();
            query.UsageAccountId = model.UsageAccountId;
            query.CurrentPage = model.CurrentPage <= 0 ? 1 : model.CurrentPage;
            query.PageSize = model.PageSize <= 0 ? 10 : model.PageSize;

            IDictionary<string, object> result = await _mediator.Send(query);
            //回傳
            ApiResultModel apiResult = new ApiResultModel();
            if (result != null)
            {
                if (result.Count > 0)
                {
                    apiResult.Code = 0;
                    apiResult.Message = string.Empty;
                    apiResult.Data = result;
                }
                else
                {
                    apiResult.Code = 1;
                    apiResult.Message = "此帳號已無更多使用金額資料!!";
                    apiResult.Data = new object();
                }
            }
            else
            {
                apiResult.Code = 404;
                apiResult.Message = "使用者帳號Id不存在!!";
                apiResult.Data = new object();
            }

            return apiResult;
        }
    }
}
