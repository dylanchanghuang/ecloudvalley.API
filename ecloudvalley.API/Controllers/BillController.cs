using ecloudvalley.Application.Models.Bill;
using ecloudvalley.Application.Service.Bill;
using ecloudvalley.Domain.Bill.Dtos;
using ecloudvalley.Infrastructure.SharedKernel.Models.Api;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ecloudvalley.API.Controllers
{
    /// <summary>
    /// 帳單API
    /// </summary>
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Produces("application/json")]
    public class BillController : ControllerBase
    {
        private readonly ILogger<BillController> _logger;
        private readonly IBillService _billService;

        public BillController(ILogger<BillController> logger, IBillService billService)
        {
            _logger = logger;
            _billService = billService;
        }

        /// <summary>
        /// 取得指定使用者帳號Id的未混和成本
        /// </summary>
        /// <param name="model">使用者帳號Id</param>
        /// <returns></returns>
        /// <response code="200">Ok</response>
        [HttpPost]
        [Route("GetUnblendedCost")]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResultModel))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        public async Task<ActionResult<ApiResultModel>> GetUnblendedCostAsync([FromBody] AccountModel model)
        {
            ApiResultModel apiResult = new ApiResultModel();
            try
            {
                apiResult = await _billService.GetUnblendedCostAsync(model);
            }
            catch //(Exception ex)
            {
                apiResult.Status500InternalServerError("[系統異常錯誤] 取得使用者帳單的未混和成本資料失敗!!");
            }
            return Ok(apiResult);
        }

        /// <summary>
        /// 取得指定使用者帳號Id的使用金額
        /// </summary>
        /// <param name="model">使用者帳號Id</param>
        /// <returns></returns>
        /// <response code="200">Ok</response>
        [HttpPost]
        [Route("GetUsageAmount")]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResultModel))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        public async Task<ActionResult<ApiResultModel>> GetUsageAmountAsync([FromBody] AccountModel model)
        {
            ApiResultModel apiResult = new ApiResultModel();
            try
            {
                apiResult = await _billService.GetUsageAmountAsync(model);
            }
            catch (Exception ex)
            {
                string message = ex.Message;
                apiResult.Status500InternalServerError("[系統異常錯誤] 取得使用者帳單的使用金額資料失敗!!");
            }
            return Ok(apiResult);
        }
    }
}
