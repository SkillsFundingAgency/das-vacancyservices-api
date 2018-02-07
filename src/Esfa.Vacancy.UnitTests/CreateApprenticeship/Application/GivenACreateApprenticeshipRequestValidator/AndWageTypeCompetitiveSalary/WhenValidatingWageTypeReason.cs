using System.Linq;
using System.Threading.Tasks;
using Esfa.Vacancy.Application.Commands.CreateApprenticeship;
using Esfa.Vacancy.Application.Commands.CreateApprenticeship.Validators;
using Esfa.Vacancy.Domain.Validation;
using FluentAssertions;
using NUnit.Framework;
using Ploeh.AutoFixture;

namespace Esfa.Vacancy.UnitTests.CreateApprenticeship.Application.GivenACreateApprenticeshipRequestValidator.AndWageTypeCompetitiveSalary
{
    [TestFixture]
    public class WhenValidatingWageTypeReason : CreateApprenticeshipRequestValidatorBase
    {
        [Test]
        public async Task AndNoValueThenIsInvalid()
        {
            var fixture = new Fixture();
            var request = new CreateApprenticeshipRequest
            {
                WageType = WageType.CompetitiveSalary
            };

            var context = GetValidationContextForProperty(request, req => req.WageTypeReason);

            var validator = fixture.Create<CreateApprenticeshipRequestValidator>();

            var result = await validator.ValidateAsync(context);

            result.IsValid.Should().Be(false);
            result.Errors.First().ErrorCode
                .Should().Be(ErrorCodes.CreateApprenticeship.WageTypeReason);
            result.Errors.First().ErrorMessage
                .Should().Be("'Wage Type Reason' should not be empty.");
        }

        [Test]
        public async Task AndEmptyStringValueThenIsInvalid()
        {
            var fixture = new Fixture();
            var request = new CreateApprenticeshipRequest
            {
                WageType = WageType.CompetitiveSalary,
                WageTypeReason = ""
            };

            var context = GetValidationContextForProperty(request, req => req.WageTypeReason);

            var validator = fixture.Create<CreateApprenticeshipRequestValidator>();

            var result = await validator.ValidateAsync(context);

            result.IsValid.Should().Be(false);
            result.Errors.First().ErrorCode
                .Should().Be(ErrorCodes.CreateApprenticeship.WageTypeReason);
            result.Errors.First().ErrorMessage
                .Should().Be("'Wage Type Reason' should not be empty.");
        }

        [Test]
        public async Task AndHasValueThenIsValid()
        {
            var fixture = new Fixture();
            var request = new CreateApprenticeshipRequest
            {
                WageType = WageType.CompetitiveSalary,
                WageTypeReason = fixture.Create<string>()
            };

            var context = GetValidationContextForProperty(request, req => req.WageTypeReason);

            var validator = fixture.Create<CreateApprenticeshipRequestValidator>();

            var result = await validator.ValidateAsync(context);

            result.IsValid.Should().Be(true);
        }
    }
}