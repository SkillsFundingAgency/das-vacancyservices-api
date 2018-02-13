using System.Linq;
using System.Threading.Tasks;
using Esfa.Vacancy.Application.Commands.CreateApprenticeship;
using Esfa.Vacancy.Application.Commands.CreateApprenticeship.Validators;
using Esfa.Vacancy.Domain.Validation;
using FluentAssertions;
using NUnit.Framework;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.AutoMoq;

namespace Esfa.Vacancy.UnitTests.CreateApprenticeship.Application.GivenACreateApprenticeshipRequestValidator.AndWageTypeCustom
{
    [TestFixture]
    public class WhenValidatingMaxWage : CreateApprenticeshipRequestValidatorBase
    {
        [Test]
        public async Task AndNoValueThenIsValid()
        {
            var fixture = new Fixture().Customize(new AutoMoqCustomization());
            var request = new CreateApprenticeshipRequest
            {
                WageType = WageType.Custom,
                MinWage = 50m
            };

            var context = GetValidationContextForProperty(request, req => req.MaxWage);

            var validator = fixture.Create<CreateApprenticeshipRequestValidator>();

            var result = await validator.ValidateAsync(context);

            result.IsValid.Should().Be(true);
        }

        [Test]
        public async Task AndHasValueThenIsValid()
        {
            var fixture = new Fixture().Customize(new AutoMoqCustomization());
            var request = new CreateApprenticeshipRequest
            {
                WageType = WageType.Custom,
                MinWage = 50m,
                MaxWage = 99999.99m
            };

            var context = GetValidationContextForProperty(request, req => req.MaxWage);

            var validator = fixture.Create<CreateApprenticeshipRequestValidator>();

            var result = await validator.ValidateAsync(context);

            result.IsValid.Should().Be(true);
        }

        [Test]
        public async Task AndValueNotMonetaryThenIsInvalid()
        {
            var fixture = new Fixture().Customize(new AutoMoqCustomization());
            var request = new CreateApprenticeshipRequest
            {
                WageType = WageType.Custom,
                MinWage = 50m,
                MaxWage = 99.99999m
            };

            var context = GetValidationContextForProperty(request, req => req.MaxWage);

            var validator = fixture.Create<CreateApprenticeshipRequestValidator>();

            var result = await validator.ValidateAsync(context);

            result.IsValid.Should().Be(false);
            result.Errors.First().ErrorCode
                .Should().Be(ErrorCodes.CreateApprenticeship.MaxWage);
            result.Errors.First().ErrorMessage
                .Should().Be("'Max Wage' must be a monetary value.");
        }

        [Test]
        public async Task AndLessThanMinWageThenIsInvalid()
        {
            var fixture = new Fixture().Customize(new AutoMoqCustomization());
            var request = new CreateApprenticeshipRequest
            {
                WageType = WageType.Custom,
                MinWage = 50m,
                MaxWage = 49.99m
            };

            var context = GetValidationContextForProperty(request, req => req.MaxWage);

            var validator = fixture.Create<CreateApprenticeshipRequestValidator>();

            var result = await validator.ValidateAsync(context);

            result.IsValid.Should().Be(false);
            result.Errors.First().ErrorCode
                .Should().Be(ErrorCodes.CreateApprenticeship.MaxWage);
            result.Errors.First().ErrorMessage
                .Should().Be(ErrorMessages.CreateApprenticeship.MaxWageCantBeLessThanMinWage);
        }
    }
}