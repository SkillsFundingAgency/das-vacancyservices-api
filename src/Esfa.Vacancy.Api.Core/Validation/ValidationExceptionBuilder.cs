using System.Collections.Generic;
using FluentValidation;
using FluentValidation.Results;

namespace Esfa.Vacancy.Api.Core.Validation
{
    public class ValidationExceptionBuilder : IValidationExceptionBuilder
    {
        public ValidationException Build(string errorCode, string errorMessage, string propertyName = "default")
        {
            return new ValidationException(new List<ValidationFailure>()
            {
                new ValidationFailure(propertyName, errorMessage){ErrorCode = errorCode}
            });
        }
    }
}