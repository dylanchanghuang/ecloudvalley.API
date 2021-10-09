using System;
using System.Collections.Generic;
using System.Text;

namespace ecloudvalley.Infrastructure.SharedKernel.Models.Api.ApiErrors
{
    public class CustomInternalServerErrorResult : ApiResultModel
    {

        public CustomInternalServerErrorResult(string message) : base(500, message)
        {
        }
    }
}
