using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ecloudvalley.Domain.Common.Exceptions
{
    public class CustomizedValidationException : Exception
    {
        public CustomizedValidationException()
            : base("One or more validation failures have occurred.")
        {
            Errors = new Dictionary<string, string[]>();
        }

        public CustomizedValidationException(IEnumerable<ValidationFailure> failures)
            : this()
        {
            var failureGroups = failures
                .GroupBy(e => e.PropertyName, e => e.ErrorMessage);

            foreach (var failureGroup in failureGroups)
            {
                var propertyName = failureGroup.Key;
                //排除同個欄位,重複的錯誤訊息
                var propertyFailures = failureGroup.Distinct().ToArray();

                Errors.Add(propertyName, propertyFailures);
            }
        }

        public IDictionary<string, string[]> Errors { get; }
    }
}
