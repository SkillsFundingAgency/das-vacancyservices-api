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

namespace Esfa.Vacancy.UnitTests.CreateApprenticeship.Application.GivenACreateApprenticeshipRequestValidator.AndWageTypeCustomWageFixed
{
    public class WhenValidatingWageUnit : CreateApprenticeshipRequestValidatorBase
    {
        private IFixture _fixture;
        private CreateApprenticeshipRequestValidator _validator;
        private Mock<IMinimumWageSelector> _mockSelector;
        private Mock<IHourlyWageCalculator> _mockCalculator;
        private decimal _expectedAllowedFixedWage;
        private decimal _expectedAttemptedFixedWage;

        [SetUp]
        public void Setup()
        {
            _fixture = new Fixture().Customize(new AutoMoqCustomization());

            _expectedAllowedFixedWage = _fixture.Create<decimal>();
            _expectedAttemptedFixedWage = _fixture.Create<decimal>();

            _mockSelector = _fixture.Freeze<Mock<IMinimumWageSelector>>();
            _mockSelector
                .Setup(selector => selector.SelectHourlyRateAsync(It.IsAny<DateTime>()))
                .ReturnsAsync(_expectedAllowedFixedWage);

            _mockCalculator = _fixture.Freeze<Mock<IHourlyWageCalculator>>();
            _mockCalculator
                .Setup(calculator => calculator.Calculate(It.IsAny<decimal>(), It.IsAny<WageUnit>(), It.IsAny<decimal>()))
                .Returns(_expectedAttemptedFixedWage);

            _validator = _fixture.Create<CreateApprenticeshipRequestValidator>();
        }

        [Test]
        public async Task AndIsNotApplicableThenIsInvalid()
        {
            var request = new CreateApprenticeshipRequest
            {
                WageType = WageType.CustomWageFixed,
                WageUnit = WageUnit.NotApplicable
            };
            var context = GetValidationContextForProperty(request, req => req.WageUnit);

            var result = await _validator.ValidateAsync(context).ConfigureAwait(false);

            result.IsValid.Should().Be(false);
            result.Errors.Count.Should().Be(1);
            result.Errors.First().ErrorCode
                .Should().Be(ErrorCodes.CreateApprenticeship.WageUnit);
            result.Errors.First().ErrorMessage
                .Should().Be("'Wage Unit' should not be equal to 'NotApplicable'.");
        }

        [Test]
        public async Task ThenUsesSelectorToGetAllowedWeeklyWage()
        {
            var expectedStartDate = _fixture.Create<DateTime>();
            var request = new CreateApprenticeshipRequest
            {
                WageType = WageType.CustomWageFixed,
                WageUnit = _fixture.Create<Generator<WageUnit>>().First(unit => unit != WageUnit.NotApplicable),
                ExpectedStartDate = expectedStartDate
            };
            var context = GetValidationContextForProperty(request, req => req.WageUnit);

            await _validator.ValidateAsync(context).ConfigureAwait(false);

            _mockSelector.Verify(selector => selector.SelectHourlyRateAsync(expectedStartDate));
        }

        [Test]
        public async Task ThenUsesCalculatorToDetermineAttemptedWeeklyWage()
        {
            var expectedStartDate = _fixture.Create<DateTime>();
            var request = new CreateApprenticeshipRequest
            {
                WageType = WageType.CustomWageFixed,
                WageUnit = _fixture.Create<Generator<WageUnit>>().First(unit => unit != WageUnit.NotApplicable),
                ExpectedStartDate = expectedStartDate
            };
            var context = GetValidationContextForProperty(request, req => req.WageUnit);

            await _validator.ValidateAsync(context).ConfigureAwait(false);

            _mockCalculator.Verify(calculator => calculator.Calculate(request.FixedWage.GetValueOrDefault(), request.WageUnit, (decimal)request.HoursPerWeek));
        }

        [Test]
        public void AndSelectorThrowsInfrastructureException_ThenLetsExceptionBubble()
        {
            var expectedStartDate = _fixture.Create<DateTime>();
            var request = new CreateApprenticeshipRequest
            {
                WageType = WageType.CustomWageFixed,
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
                WageType = WageType.CustomWageFixed,
                WageUnit = _fixture.Create<Generator<WageUnit>>().First(unit => unit != WageUnit.NotApplicable),
                ExpectedStartDate = expectedStartDate
            };
            var context = GetValidationContextForProperty(request, req => req.WageUnit);

            _mockCalculator
                .Setup(calculator => calculator.Calculate(It.IsAny<decimal>(), It.IsAny<WageUnit>(), It.IsAny<decimal>()))
                .Throws<ArgumentOutOfRangeException>();

            var result = await _validator.ValidateAsync(context).ConfigureAwait(false);

            result.IsValid.Should().Be(false);
            result.Errors.First().ErrorCode
                .Should().Be(ErrorCodes.CreateApprenticeship.FixedWage);
        }

        private static List<TestCaseData> TestCases => new List<TestCaseData>
        {
            new TestCaseData(WageUnit.Weekly, 3.5m, 125.99m, false).SetName("And attempted weekly is less than allowed Then is invalid"),
            new TestCaseData(WageUnit.Weekly, 3.5m, 126.00m, true).SetName("And attempted weekly is same as allowed Then is valid"),
            new TestCaseData(WageUnit.Weekly, 3.5m, 126.01m, true).SetName("And attempted weekly is greater than allowed Then is valid"),
            new TestCaseData(WageUnit.Monthly, 3.5m, 545.99m, false).SetName("And attempted monthly is less than allowed Then is invalid"),
            new TestCaseData(WageUnit.Monthly, 3.5m, 546.00m, true).SetName("And attempted monthly is same as allowed Then is valid"),
            new TestCaseData(WageUnit.Monthly, 3.5m, 546.01m, true).SetName("And attempted monthly is greater than allowed Then is valid"),
            new TestCaseData(WageUnit.Annually, 3.5m, 6551.99m, false).SetName("And attempted annually is less than allowed Then is invalid"),
            new TestCaseData(WageUnit.Annually, 3.5m, 6552.00m, true).SetName("And attempted annually is same as allowed Then is valid"),
            new TestCaseData(WageUnit.Annually, 3.5m, 6552.01m, true).SetName("And attempted annually is greater than allowed Then is valid")
        };

        [TestCaseSource(nameof(TestCases))]
        public async Task AndCheckingAllowedVersusAttemtpedWeeklyWage(WageUnit wageUnit, decimal allowedMinimumHourlyWage, decimal attemptedFixedWage, bool expectedIsValid)
        {
            var request = new CreateApprenticeshipRequest
            {
                WageType = WageType.CustomWageFixed,
                WageUnit = wageUnit,
                FixedWage = attemptedFixedWage,
                HoursPerWeek = 36,
                ExpectedStartDate = _fixture.Create<DateTime>()
            };
            var context = GetValidationContextForProperty(request, req => req.WageUnit);

            _fixture = new Fixture().Customize(new AutoMoqCustomization());
            _mockSelector = _fixture.Freeze<Mock<IMinimumWageSelector>>();
            _mockSelector
                .Setup(selector => selector.SelectHourlyRateAsync(It.IsAny<DateTime>()))
                .ReturnsAsync(allowedMinimumHourlyWage);

            _fixture.Inject<IHourlyWageCalculator>(new HourlyWageCalculator());

            _validator = _fixture.Create<CreateApprenticeshipRequestValidator>();

            var result = await _validator.ValidateAsync(context).ConfigureAwait(false);

            result.IsValid.Should().Be(expectedIsValid);
            if (!result.IsValid)
            {
                result.Errors.First().ErrorCode
                    .Should().Be(ErrorCodes.CreateApprenticeship.FixedWage);
                result.Errors.First().ErrorMessage
                    .Should().Be(ErrorMessages.CreateApprenticeship.FixedWageIsBelowApprenticeMinimumWage);
            }
        }
    }
}
