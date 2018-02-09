using Esfa.Vacancy.Application.Commands.CreateApprenticeship;
using Esfa.Vacancy.Application.Commands.CreateApprenticeship.Validators;
using Esfa.Vacancy.Domain.Validation;
using FluentValidation.TestHelper;
using NUnit.Framework;

namespace Esfa.Vacancy.UnitTests.CreateApprenticeship.Application.GivenACreateApprenticeshipRequestValidator.
    WhenValidatingApplicationMethod
{
    [TestFixture]
    public class AndApplicationMethodIsOffline
    {
        [Test]
        public void ThenTheRequestIsInvalid()
        {
            var request = new CreateApprenticeshipRequest
            {
                ApplicationMethod = ApplicationMethod.Offline
            };

            var sut = new CreateApprenticeshipRequestValidator();
            sut.Validate(request);

            sut.ShouldHaveValidationErrorFor(req => req.ApplicationMethod, request)
               .WithErrorCode(ErrorCodes.CreateApprenticeship.ApplicationMethod)
               .WithErrorMessage("Invalid Application Method");
        }
    }
}
