using System.Linq;
using System.Threading.Tasks;
using Esfa.Vacancy.Application.Commands.CreateApprenticeship;
using Esfa.Vacancy.Application.Commands.CreateApprenticeship.Validators;
using Esfa.Vacancy.Domain.Validation;
using FluentAssertions;
using NUnit.Framework;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.AutoMoq;

namespace Esfa.Vacancy.UnitTests.CreateApprenticeship.Application.GivenACreateApprenticeshipRequestValidator.AndWageTypeCustomWageRange
{
    [TestFixture]
    public class WhenValidatingWageTypeReason : CreateApprenticeshipRequestValidatorBase
    {
        [Test]
        public async Task AndHasValueThenIsInvalid()
        {
            var fixture = new Fixture().Customize(new AutoMoqCustomization());
            var request = new CreateApprenticeshipRequest
            {
                WageType = WageType.CustomWageRange,
                WageTypeReason = fixture.Create<string>()
            };

            var context = GetValidationContextForProperty(request, req => req.WageTypeReason);

            var validator = fixture.Create<CreateApprenticeshipRequestValidator>();

            var result = await validator.ValidateAsync(context);

            result.IsValid.Should().Be(false);
            result.Errors.First().ErrorCode
                .Should().Be(ErrorCodes.CreateApprenticeship.WageTypeReason);
            result.Errors.First().ErrorMessage
                .Should().Be("'Wage Type Reason' must be empty.");
        }

        [Test]
        public async Task AndNoValueThenIsValid()
        {
            var fixture = new Fixture().Customize(new AutoMoqCustomization());
            var request = new CreateApprenticeshipRequest
            {
                WageType = WageType.CustomWageRange
            };

            var context = GetValidationContextForProperty(request, req => req.WageTypeReason);

            var validator = fixture.Create<CreateApprenticeshipRequestValidator>();

            var result = await validator.ValidateAsync(context);

            result.IsValid.Should().Be(true);
        }
    }
}