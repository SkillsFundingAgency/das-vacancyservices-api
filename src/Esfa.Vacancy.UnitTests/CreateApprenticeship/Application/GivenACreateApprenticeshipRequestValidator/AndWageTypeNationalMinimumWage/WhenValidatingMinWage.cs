using System.Linq;
using System.Threading.Tasks;
using Esfa.Vacancy.Application.Commands.CreateApprenticeship;
using Esfa.Vacancy.Application.Commands.CreateApprenticeship.Validators;
using Esfa.Vacancy.Domain.Validation;
using FluentAssertions;
using NUnit.Framework;
using Ploeh.AutoFixture;

namespace Esfa.Vacancy.UnitTests.CreateApprenticeship.Application.GivenACreateApprenticeshipRequestValidator.AndWageTypeNationalMinimumWage
{
    [TestFixture]
    public class WhenValidatingMinWage : CreateApprenticeshipRequestValidatorBase
    {
        [Test]
        public async Task AndHasValueThenIsInvalid()
        {
            var fixture = new Fixture();
            var request = new CreateApprenticeshipRequest
            {
                WageType = WageType.NationalMinimumWage,
                MinWage = fixture.Create<decimal>()
            };

            var context = GetValidationContextForProperty(request, req => req.MinWage);

            var validator = fixture.Create<CreateApprenticeshipRequestValidator>();

            var result = await validator.ValidateAsync(context);

            result.IsValid.Should().Be(false);
            result.Errors.First().ErrorCode
                .Should().Be(ErrorCodes.CreateApprenticeship.MinWage);
            result.Errors.First().ErrorMessage
                .Should().Be("'Min Wage' must be empty.");
        }

        [Test]
        public async Task AndNoValueThenIsValid()
        {
            var fixture = new Fixture();
            var request = new CreateApprenticeshipRequest
            {
                WageType = WageType.NationalMinimumWage
            };

            var context = GetValidationContextForProperty(request, req => req.MinWage);

            var validator = fixture.Create<CreateApprenticeshipRequestValidator>();

            var result = await validator.ValidateAsync(context);

            result.IsValid.Should().Be(true);
        }
    }
}