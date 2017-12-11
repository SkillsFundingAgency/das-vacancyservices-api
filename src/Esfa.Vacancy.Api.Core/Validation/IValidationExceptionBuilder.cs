using FluentValidation;

namespace Esfa.Vacancy.Api.Core.Validation
{
    public interface IValidationExceptionBuilder
    {
        ValidationException Build(string errorCode, string errorMessage, string propertyName = "default");
    }
}