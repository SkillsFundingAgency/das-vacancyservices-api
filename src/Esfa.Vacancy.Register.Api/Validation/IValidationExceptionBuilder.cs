using FluentValidation;

namespace Esfa.Vacancy.Register.Api.Validation
{
    public interface IValidationExceptionBuilder
    {
        ValidationException Build(string errorCode, string errorMessage, string propertyName = "default");
    }
}