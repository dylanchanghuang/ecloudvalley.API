using System;
using System.Collections.Generic;
using System.Text;

namespace ecloudvalley.Infrastructure.SharedKernel.Models.Api.ApiErrors
{
    public class CustomNotFoundResult : ApiResultModel
    {

        public CustomNotFoundResult(string message) : base(404, message)
        {
        }
    }
}
