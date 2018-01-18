using Esfa.Vacancy.Application.Commands.CreateApprenticeship;
using Esfa.Vacancy.Application.Commands.CreateApprenticeship.Validators;
using Esfa.Vacancy.Domain.Validation;
using FluentValidation.TestHelper;
using NUnit.Framework;

namespace Esfa.Vacancy.UnitTests.CreateApprenticeship.Application.GivenACreateApprenticeshipRequestValidator
{
    [TestFixture]
    public class WhenValidatingLocationType
    {
        [Test]
        public void ThenLocationTypeIsRequired()
        {
            var request = new CreateApprenticeshipRequest();

            var validator = new CreateApprenticeshipRequestValidator();

            validator
                .ShouldHaveValidationErrorFor(x => x.LocationType, request)
                .WithErrorCode(ErrorCodes.CreateApprenticeship.LocationTypeIsRequired);
        }

        [Test]
        public void ThenLocationTypeIsValid()
        {
            var request = new CreateApprenticeshipRequest()
            {
                LocationType = LocationType.OtherLocation
            };

            var validator = new CreateApprenticeshipRequestValidator();

            validator
                .ShouldNotHaveValidationErrorFor(x => x.LocationType, request);

        }
    }
}
