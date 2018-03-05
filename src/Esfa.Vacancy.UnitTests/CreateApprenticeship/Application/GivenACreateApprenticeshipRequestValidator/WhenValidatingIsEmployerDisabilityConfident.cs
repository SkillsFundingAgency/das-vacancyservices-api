using Esfa.Vacancy.Application.Commands.CreateApprenticeship;
using Esfa.Vacancy.Application.Commands.CreateApprenticeship.Validators;
using Esfa.Vacancy.Domain.Validation;
using FluentValidation.TestHelper;
using NUnit.Framework;
using NUnit.Framework.Internal;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.AutoMoq;

namespace Esfa.Vacancy.UnitTests.CreateApprenticeship.Application.GivenACreateApprenticeshipRequestValidator
{
    [TestFixture]
    public class WhenValidatingIsEmployerDisabilityConfident
    {
        [Test]
        public void AndIsProvidedThenIsValid()
        {
            var fixture = new Fixture().Customize(new AutoMoqCustomization());

            var request = fixture.Build<CreateApprenticeshipRequest>()
                .With(req => req.IsEmployerDisabilityConfident, fixture.Create<bool>())
                .Create();

            var sut = fixture.Create<CreateApprenticeshipRequestValidator>();

            sut.ShouldNotHaveValidationErrorFor(req => req.IsEmployerDisabilityConfident, request);
        }

        [Test]
        public void AndIsNotProvidedThenIsInvalid()
        {
            var fixture = new Fixture().Customize(new AutoMoqCustomization());

            var request = fixture.Build<CreateApprenticeshipRequest>()
                .With(req => req.IsEmployerDisabilityConfident, null)
                .Create();

            var sut = fixture.Create<CreateApprenticeshipRequestValidator>();

            sut.ShouldHaveValidationErrorFor(req => req.IsEmployerDisabilityConfident, request)
                .WithErrorMessage("'Is Employer Disability Confident' must not be empty.")
                .WithErrorCode(ErrorCodes.CreateApprenticeship.IsEmployerDisabilityConfident);
        }

    }
}