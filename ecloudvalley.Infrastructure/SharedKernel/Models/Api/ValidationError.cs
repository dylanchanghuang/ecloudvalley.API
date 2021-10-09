using System;
using System.Collections.Generic;
using System.Text;

namespace ecloudvalley.Infrastructure.SharedKernel.Models.Api
{
    public class ValidationError
    {
        //[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        /// <summary>
        /// 錯誤欄位
        /// </summary>
        public string Field { get; }
        /// <summary>
        /// 錯誤訊息
        /// </summary>
        public string Message { get; }

        public ValidationError(string field, string message)
        {
            //Field = field != string.Empty ? field : null;
            Field = field;
            Message = message;
        }
    }
}
