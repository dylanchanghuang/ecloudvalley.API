using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ecloudvalley.Infrastructure.SharedKernel.Models.Api
{
    /// <summary>
    /// [回傳] API傳回的資料Model 
    /// </summary>
    public class ApiResultModel
    {
        /// <summary>
        /// 錯誤代碼 (大於0表示有錯誤) (固定錯誤 401:沒有權限; 404:找不到資料; 500:系統出錯)
        /// </summary>
        public int Code { get; set; } = 0;

        /// <summary>
        /// 錯誤訊息
        /// </summary>
        public string Message { get; set; } = string.Empty;

        /// <summary>
        /// 回傳的資料
        /// </summary>
        public dynamic Data { get; set; } = new object();

        /// <summary>
        /// 傳入欄位的檢查結果－錯誤欄位和錯誤訊息 (null轉到json不顯示)
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<ValidationError> Errors { get; set; }

        public ApiResultModel()
        {
        }

        public ApiResultModel(int _code, string _message)
        {
            Code = _code;
            Message = _message;
        }

        public void Status401Unauthorized(string _message)
        {
            Code = 401;
            Message = _message;
        }

        public void Status404NotFound(string _message)
        {
            Code = 404;
            Message = _message;
        }

        public void Status500InternalServerError(string _message)
        {
            Code = 500;
            Message = _message;
        }
    }

    public class ApiResultModel<T> where T : new()
    {
        /// <summary>
        /// 錯誤代碼 (大於0表示有錯誤) (固定錯誤 401:沒有權限; 404:找不到資料; 500:系統出錯)
        /// </summary>
        public int Code { get; set; } = 0;

        /// <summary>
        /// 錯誤訊息
        /// </summary>
        public string Message { get; set; } = string.Empty;

        /// <summary>
        /// 回傳的資料
        /// </summary>
        public T Data { get; set; }

        /// <summary>
        /// 傳入欄位的檢查結果－錯誤欄位和錯誤訊息 (null轉到json不顯示)
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<ValidationError> Errors { get; set; }
    }
}
