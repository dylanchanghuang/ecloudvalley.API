using System;
using System.Collections.Generic;
using System.Text;

namespace ecloudvalley.Infrastructure.SharedKernel.Models.Api.ApiErrors
{
    public class CustomUnauthorizedResult : ApiResultModel
    {
        public CustomUnauthorizedResult(string message) : base(401, message)
        {
        }
    }
}
