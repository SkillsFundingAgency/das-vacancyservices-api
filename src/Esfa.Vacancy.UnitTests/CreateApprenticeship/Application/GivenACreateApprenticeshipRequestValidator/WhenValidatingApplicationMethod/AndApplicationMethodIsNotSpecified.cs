using Esfa.Vacancy.Application.Commands.CreateApprenticeship;
using Esfa.Vacancy.Application.Commands.CreateApprenticeship.Validators;
using Esfa.Vacancy.Domain.Validation;
using FluentValidation.TestHelper;
using NUnit.Framework;

namespace Esfa.Vacancy.UnitTests.CreateApprenticeship.Application.GivenACreateApprenticeshipRequestValidator.
    WhenValidatingApplicationMethod
{
    [TestFixture]
    public class AndApplicationMethodIsNotSpecified
    {
        [Test]
        public void ThenRequestIsNotValid()
        {
            var request = new CreateApprenticeshipRequest
            {
                ApplicationMethod = ApplicationMethod.NotSpecified
            };

            var sut = new CreateApprenticeshipRequestValidator();
            sut.Validate(request);

            sut.ShouldHaveValidationErrorFor(req => req.ApplicationMethod, request)
               .WithErrorCode(ErrorCodes.CreateApprenticeship.ApplicationMethod)
               .WithErrorMessage("'Application Method' should not be empty.");
        }
    }
}
