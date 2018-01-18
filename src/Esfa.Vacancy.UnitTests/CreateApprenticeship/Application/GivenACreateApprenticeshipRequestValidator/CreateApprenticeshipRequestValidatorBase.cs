using System;
using System.Linq.Expressions;
using FluentValidation;
using FluentValidation.Internal;

namespace Esfa.Vacancy.UnitTests.CreateApprenticeship.Application.GivenACreateApprenticeshipRequestValidator
{
    public abstract class CreateApprenticeshipRequestValidatorBase
    {
        protected ValidationContext<T> GetValidationContextForProperty<T, TProperty>(T objectToValidate, Expression<Func<T, TProperty>> propertyPicker)
        {
            return new ValidationContext<T>(objectToValidate, new PropertyChain(), new MemberNameValidatorSelector(new[] { propertyPicker.GetMember().Name }));
        }
    }
}
