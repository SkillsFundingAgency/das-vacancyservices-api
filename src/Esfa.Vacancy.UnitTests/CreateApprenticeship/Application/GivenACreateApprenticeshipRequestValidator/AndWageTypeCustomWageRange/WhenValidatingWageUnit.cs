using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Esfa.Vacancy.Application.Commands.CreateApprenticeship;
using Esfa.Vacancy.Application.Commands.CreateApprenticeship.Validators;
using Esfa.Vacancy.Domain.Validation;
using Esfa.Vacancy.Infrastructure.Exceptions;
using FluentAssertions;
using FluentValidation.Results;
using Moq;
using NUnit.Framework;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.AutoMoq;

namespace Esfa.Vacancy.UnitTests.CreateApprenticeship.Application.GivenACreateApprenticeshipRequestValidator.AndWageTypeCustomWageRange
{
    [TestFixture]
    public class WhenValidatingWageUnit : CreateApprenticeshipRequestValidatorBase
    {
        private IFixture _fixture;
        private CreateApprenticeshipRequestValidator _validator;
        private Mock<IMinimumWageSelector> _mockSelector;
        private Mock<IMinimumWageCalculator> _mockCalculator;
        private decimal _expectedAllowedMinimumWage;
        private decimal _expectedAttemptedMinimumWage;

        [SetUp]
        public void Setup()
        {
            _fixture = new Fixture().Customize(new AutoMoqCustomization());

            _expectedAllowedMinimumWage = _fixture.Create<decimal>();
            _expectedAttemptedMinimumWage = _fixture.Create<decimal>();

            _mockSelector = _fixture.Freeze<Mock<IMinimumWageSelector>>();
            _mockSelector
                .Setup(selector => selector.SelectHourlyRateAsync(It.IsAny<DateTime>()))
                .ReturnsAsync(_expectedAllowedMinimumWage);

            _mockCalculator = _fixture.Freeze<Mock<IMinimumWageCalculator>>();
            _mockCalculator
                .Setup(calculator => calculator.CalculateMinimumWage(It.IsAny<decimal>(), It.IsAny<WageUnit>(), It.IsAny<decimal>()))
                .Returns(_expectedAttemptedMinimumWage);

            _validator = _fixture.Create<CreateApprenticeshipRequestValidator>();
        }

        [Test]
        public async Task AndIsNotApplicableThenIsInvalid()
        {
            var request = new CreateApprenticeshipRequest
            {
                WageType = WageType.CustomWageRange,
                WageUnit = WageUnit.NotApplicable
            };
            var context = GetValidationContextForProperty(request, req => req.WageUnit);

            var result = await _validator.ValidateAsync(context);

            result.IsValid.Should().Be(false);
            result.Errors.Count.Should().Be(1);
            result.Errors.First().ErrorCode
                .Should().Be(ErrorCodes.CreateApprenticeship.WageUnit);
            result.Errors.First().ErrorMessage
                .Should().Be("'Wage Unit' should not be equal to 'NotApplicable'.");
        }

        [Test]
        public async Task ThenUsesSelectorToGetAllowedMinimumWage()
        {
            var expectedStartDate = _fixture.Create<DateTime>();
            var request = new CreateApprenticeshipRequest
            {
                WageType = WageType.CustomWageRange,
                WageUnit = _fixture.Create<Generator<WageUnit>>().First(unit => unit != WageUnit.NotApplicable),
                ExpectedStartDate = expectedStartDate,
                MinWage = _fixture.Create<decimal>()
            };
            var context = GetValidationContextForProperty(request, req => req.WageUnit);

            await _validator.ValidateAsync(context);

            _mockSelector.Verify(selector => selector.SelectHourlyRateAsync(expectedStartDate));
        }

        [Test]
        public async Task ThenUsesCalculatorToDetermineAttemptedMinimumWage()
        {
            var expectedStartDate = _fixture.Create<DateTime>();
            var request = new CreateApprenticeshipRequest
            {
                WageType = WageType.CustomWageRange,
                WageUnit = _fixture.Create<Generator<WageUnit>>().First(unit => unit != WageUnit.NotApplicable),
                ExpectedStartDate = expectedStartDate,
                MinWage = _fixture.Create<decimal>()
            };
            var context = GetValidationContextForProperty(request, req => req.WageUnit);

            await _validator.ValidateAsync(context);

            _mockCalculator.Verify(calculator => calculator.CalculateMinimumWage(request.MinWage.Value, request.WageUnit, (decimal)request.HoursPerWeek));
        }

        [Test]
        public void AndSelectorThrowsInfrastructureException_ThenLetsExceptionBubble()
        {
            var expectedStartDate = _fixture.Create<DateTime>();
            var request = new CreateApprenticeshipRequest
            {
                WageType = WageType.CustomWageRange,
                WageUnit = _fixture.Create<Generator<WageUnit>>().First(unit => unit != WageUnit.NotApplicable),
                ExpectedStartDate = expectedStartDate
            };
            var context = GetValidationContextForProperty(request, req => req.WageUnit);

            _mockSelector
                .Setup(selector => selector.SelectHourlyRateAsync(It.IsAny<DateTime>()))
                .Throws<InfrastructureException>();

            var action = new Func<Task<ValidationResult>>(() => _validator.ValidateAsync(context));

            action.ShouldThrow<InfrastructureException>();
        }

        [Test]
        public async Task AndCalculatorThrowsException_ThenReturnsValidationResult()
        {
            var expectedStartDate = _fixture.Create<DateTime>();
            var request = new CreateApprenticeshipRequest
            {
                WageType = WageType.CustomWageRange,
                WageUnit = _fixture.Create<Generator<WageUnit>>().First(unit => unit != WageUnit.NotApplicable),
                ExpectedStartDate = expectedStartDate
            };
            var context = GetValidationContextForProperty(request, req => req.WageUnit);

            _mockCalculator
                .Setup(calculator => calculator.CalculateMinimumWage(It.IsAny<decimal>(), It.IsAny<WageUnit>(), It.IsAny<decimal>()))
                .Throws<ArgumentOutOfRangeException>();

            var result = await _validator.ValidateAsync(context);

            result.IsValid.Should().Be(false);
            result.Errors.First().ErrorCode
                .Should().Be(ErrorCodes.CreateApprenticeship.MinWage);
        }

        private static List<TestCaseData> TestCases => new List<TestCaseData>
        {
            new TestCaseData(3.5m, 125.99m, false).SetName("And attempted is less than allowed Then is invalid"),
            new TestCaseData(3.5m, 126.0m, true).SetName("And attempted is same as allowed Then is valid"),
            new TestCaseData(3.5m, 126.01m, true).SetName("And attempted is greater than allowed Then is valid")
        };

        [TestCaseSource(nameof(TestCases))]
        public async Task AndCheckingAllowedVersusAttemtpedMinWage(decimal allowedMinimumHourlyWage, decimal attemptedMinWage, bool expectedIsValid)
        {
            var request = new CreateApprenticeshipRequest
            {
                WageType = WageType.CustomWageRange,
                WageUnit = WageUnit.Weekly,
                HoursPerWeek = 36,
                MinWage = attemptedMinWage,
                ExpectedStartDate = _fixture.Create<DateTime>()
            };
            var context = GetValidationContextForProperty(request, req => req.WageUnit);

            _fixture = new Fixture().Customize(new AutoMoqCustomization());
            _mockSelector = _fixture.Freeze<Mock<IMinimumWageSelector>>();
            _mockSelector
                .Setup(selector => selector.SelectHourlyRateAsync(It.IsAny<DateTime>()))
                .ReturnsAsync(allowedMinimumHourlyWage);

            _fixture.Inject<IMinimumWageCalculator>(new MinimumWageCalculator());
            _validator = _fixture.Create<CreateApprenticeshipRequestValidator>();

            var result = await _validator.ValidateAsync(context);

            result.IsValid.Should().Be(expectedIsValid);
            if (!result.IsValid)
            {
                result.Errors.First().ErrorCode
                    .Should().Be(ErrorCodes.CreateApprenticeship.MinWage);
                result.Errors.First().ErrorMessage
                    .Should().Be(ErrorMessages.CreateApprenticeship.MinWageIsBelowApprenticeMinimumWage);
            }
        }
    }
}