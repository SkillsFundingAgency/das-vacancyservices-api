using System.Linq;
using System.Threading.Tasks;
using Esfa.Vacancy.Application.Commands.CreateApprenticeship;
using Esfa.Vacancy.Application.Commands.CreateApprenticeship.Validators;
using Esfa.Vacancy.Domain.Validation;
using FluentAssertions;
using NUnit.Framework;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.AutoMoq;

namespace Esfa.Vacancy.UnitTests.CreateApprenticeship.Application.GivenACreateApprenticeshipRequestValidator.AndWageTypeApprenticeshipMinimumWage
{
    [TestFixture]
    public class WhenValidatingWageUnit : CreateApprenticeshipRequestValidatorBase
    {
        [Test]
        public async Task AndIsNotNotApplicableThenIsInvalid()
        {
            var fixture = new Fixture().Customize(new AutoMoqCustomization());
            var request = new CreateApprenticeshipRequest
            {
                WageType = WageType.ApprenticeshipMinimumWage,
                WageUnit = fixture.Create<Generator<WageUnit>>().First(unit => unit != WageUnit.NotApplicable)
            };

            var context = GetValidationContextForProperty(request, req => req.WageUnit);

            var validator = fixture.Create<CreateApprenticeshipRequestValidator>();

            var result = await validator.ValidateAsync(context);

            result.IsValid.Should().Be(false);
            result.Errors.First().ErrorCode
                .Should().Be(ErrorCodes.CreateApprenticeship.WageUnit);
            result.Errors.First().ErrorMessage
                .Should().Be("'Wage Unit' should be equal to 'NotApplicable'.");
        }

        [Test]
        public async Task AndIsNotApplicableThenIsValid()
        {
            var fixture = new Fixture().Customize(new AutoMoqCustomization());
            var request = new CreateApprenticeshipRequest
            {
                WageType = WageType.ApprenticeshipMinimumWage,
                WageUnit = WageUnit.NotApplicable
            };

            var context = GetValidationContextForProperty(request, req => req.WageUnit);

            var validator = fixture.Create<CreateApprenticeshipRequestValidator>();

            var result = await validator.ValidateAsync(context);

            result.IsValid.Should().Be(true);
        }
    }
}