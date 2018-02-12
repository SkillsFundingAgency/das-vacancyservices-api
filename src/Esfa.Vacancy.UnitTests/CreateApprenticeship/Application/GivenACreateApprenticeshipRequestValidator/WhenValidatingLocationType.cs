using Esfa.Vacancy.Application.Commands.CreateApprenticeship;
using Esfa.Vacancy.Application.Commands.CreateApprenticeship.Validators;
using Esfa.Vacancy.Domain.Validation;
using FluentValidation.TestHelper;
using NUnit.Framework;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.AutoMoq;

namespace Esfa.Vacancy.UnitTests.CreateApprenticeship.Application.GivenACreateApprenticeshipRequestValidator
{
    [TestFixture]
    public class WhenValidatingLocationType
    {
        [Test]
        public void ThenLocationTypeIsRequired()
        {
            var request = new CreateApprenticeshipRequest();

            var fixture = new Fixture().Customize(new AutoMoqCustomization());
            var sut = fixture.Create<CreateApprenticeshipRequestValidator>();

            sut
                .ShouldHaveValidationErrorFor(x => x.LocationType, request)
                .WithErrorCode(ErrorCodes.CreateApprenticeship.LocationType);
        }

        [Test]
        public void ThenLocationTypeIsValid()
        {
            var request = new CreateApprenticeshipRequest()
            {
                LocationType = LocationType.OtherLocation
            };

            var fixture = new Fixture().Customize(new AutoMoqCustomization());
            var sut = fixture.Create<CreateApprenticeshipRequestValidator>();

            sut
                .ShouldNotHaveValidationErrorFor(x => x.LocationType, request);

        }
    }
}
