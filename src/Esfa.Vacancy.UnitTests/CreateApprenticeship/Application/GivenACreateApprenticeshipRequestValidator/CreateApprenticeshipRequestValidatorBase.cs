using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Esfa.Vacancy.Application.Commands.CreateApprenticeship;
using FluentValidation;
using FluentValidation.Internal;

namespace Esfa.Vacancy.UnitTests.CreateApprenticeship.Application.GivenACreateApprenticeshipRequestValidator
{
    public abstract class CreateApprenticeshipRequestValidatorBase
    {
        protected ValidationContext<T> GetValidationContextForProperty<T>(T objectToValidate, string propertyNameToValidate)
        {
            return new ValidationContext<T>(objectToValidate, new PropertyChain(), new MemberNameValidatorSelector(new[] { propertyNameToValidate }));
        }       
    }
}
