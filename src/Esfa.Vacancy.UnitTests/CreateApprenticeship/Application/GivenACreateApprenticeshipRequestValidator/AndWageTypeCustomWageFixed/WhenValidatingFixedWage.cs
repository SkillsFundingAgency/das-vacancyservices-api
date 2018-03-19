using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Esfa.Vacancy.Application.Commands.CreateApprenticeship;
using Esfa.Vacancy.Application.Commands.CreateApprenticeship.Validators;
using Esfa.Vacancy.Domain.Validation;
using Esfa.Vacancy.Infrastructure.Exceptions;
using Esfa.Vacancy.UnitTests.Extensions;
using FluentAssertions;
using FluentValidation.Results;
using Moq;
using NUnit.Framework;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.AutoMoq;

namespace Esfa.Vacancy.UnitTests.CreateApprenticeship.Application.GivenACreateApprenticeshipRequestValidator.AndWageTypeCustomWageFixed
{
    [TestFixture]
    public class WhenValidatingFixedWage : CreateApprenticeshipRequestValidatorBase
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
        public async Task AndNoValueThenIsInvalid()
        {
            var request = new CreateApprenticeshipRequest
            {
                WageType = WageType.CustomWageFixed
            };

            var context = GetValidationContextForProperty(request, req => req.FixedWage);

            var result = await _validator.ValidateAsync(context).ConfigureAwait(false);

            result.IsValid.Should().Be(false);
            result.Errors.Count
                .Should().Be(1);
            result.Errors.First().ErrorCode
                .Should().Be(ErrorCodes.CreateApprenticeship.FixedWage);
            result.Errors.First().ErrorMessage
                .Should().Be("'Fixed Wage' must not be empty.");
        }

        [Test]
        public async Task AndHasValueThenIsValid()
        {
            var request = new CreateApprenticeshipRequest
            {
                WageType = WageType.CustomWageFixed,
                FixedWage = 99.99m
            };

            var context = GetValidationContextForProperty(request, req => req.FixedWage);

            var result = await _validator.ValidateAsync(context).ConfigureAwait(false);

            result.IsValid.Should().Be(true);
        }

        [Test]
        public async Task AndValueNotMonetaryThenIsInvalid()
        {
            var request = new CreateApprenticeshipRequest
            {
                WageType = WageType.CustomWageFixed,
                FixedWage = 99.99999m
            };

            var context = GetValidationContextForProperty(request, req => req.FixedWage);

            var result = await _validator.ValidateAsync(context).ConfigureAwait(false);

            result.IsValid.Should().Be(false);
            result.Errors.First().ErrorCode
                .Should().Be(ErrorCodes.CreateApprenticeship.FixedWage);
            result.Errors.First().ErrorMessage
                .Should().Be("'Fixed Wage' must be a monetary value.");
        }

        [Test]
        public async Task AndExpectedStartDateIsLessThanToday_ThenIsValid()
        {
            var request = new CreateApprenticeshipRequest
            {
                WageType = WageType.CustomWageFixed,
                FixedWage = _fixture.Create<decimal>(),
                WageUnit = _fixture.CreateAnyWageUnitOtherThanNotApplicable(),
                ExpectedStartDate = DateTime.Today.AddDays(-1),
                HoursPerWeek = _fixture.Create<double>()
            };
            var context = GetValidationContextForProperty(request, req => req.FixedWage);

            var result = await _validator.ValidateAsync(context).ConfigureAwait(false);

            result.IsValid.Should().Be(true);
        }

        [Test]
        public async Task AndWageUnitIsNotApplicable_ThenIsValid()
        {
            var request = new CreateApprenticeshipRequest
            {
                WageType = WageType.CustomWageFixed,
                FixedWage = _fixture.Create<decimal>(),
                WageUnit = WageUnit.NotApplicable,
                ExpectedStartDate = _fixture.CreateFutureDateTime(),
                HoursPerWeek = _fixture.Create<double>()
            };
            var context = GetValidationContextForProperty(request, req => req.FixedWage);

            var result = await _validator.ValidateAsync(context).ConfigureAwait(false);

            result.IsValid.Should().Be(true);
        }

        [Test]
        public async Task AndHoursPerWeekIsInvalid_ThenIsValid()
        {
            var request = new CreateApprenticeshipRequest
            {
                WageType = WageType.CustomWageFixed,
                FixedWage = _fixture.Create<decimal>(),
                WageUnit = _fixture.CreateAnyWageUnitOtherThanNotApplicable(),
                ExpectedStartDate = _fixture.CreateFutureDateTime(),
                HoursPerWeek = -4
            };
            var context = GetValidationContextForProperty(request, req => req.FixedWage);

            var result = await _validator.ValidateAsync(context).ConfigureAwait(false);

            result.IsValid.Should().Be(true);
        }

        [Test]
        public async Task ThenUsesSelectorToGetAllowedWeeklyWage()
        {
            var expectedStartDate = _fixture.CreateFutureDateTime();
            var request = new CreateApprenticeshipRequest
            {
                WageType = WageType.CustomWageFixed,
                FixedWage = _fixture.Create<decimal>(),
                WageUnit = _fixture.CreateAnyWageUnitOtherThanNotApplicable(),
                ExpectedStartDate = expectedStartDate,
                HoursPerWeek = _fixture.Create<double>()
            };
            var context = GetValidationContextForProperty(request, req => req.FixedWage);

            await _validator.ValidateAsync(context).ConfigureAwait(false);

            _mockSelector.Verify(selector => selector.SelectHourlyRateAsync(expectedStartDate));
        }

        [Test]
        public async Task ThenUsesCalculatorToDetermineAttemptedWeeklyWage()
        {
            var request = new CreateApprenticeshipRequest
            {
                WageType = WageType.CustomWageFixed,
                FixedWage = _fixture.Create<decimal>(),
                WageUnit = _fixture.CreateAnyWageUnitOtherThanNotApplicable(),
                ExpectedStartDate = _fixture.CreateFutureDateTime(),
                HoursPerWeek = _fixture.Create<double>()
            };
            var context = GetValidationContextForProperty(request, req => req.FixedWage);

            await _validator.ValidateAsync(context).ConfigureAwait(false);

            _mockCalculator.Verify(calculator => calculator.Calculate(request.FixedWage.GetValueOrDefault(), request.WageUnit, (decimal)request.HoursPerWeek));
        }

        [Test]
        public void AndSelectorThrowsInfrastructureException_ThenLetsExceptionBubble()
        {
            var request = new CreateApprenticeshipRequest
            {
                WageType = WageType.CustomWageFixed,
                FixedWage = _fixture.Create<decimal>(),
                WageUnit = _fixture.CreateAnyWageUnitOtherThanNotApplicable(),
                ExpectedStartDate = _fixture.CreateFutureDateTime(),
                HoursPerWeek = _fixture.Create<double>()
            };
            var context = GetValidationContextForProperty(request, req => req.FixedWage);

            _mockSelector
                .Setup(selector => selector.SelectHourlyRateAsync(It.IsAny<DateTime>()))
                .Throws<InfrastructureException>();

            var action = new Func<Task<ValidationResult>>(() => _validator.ValidateAsync(context));

            action.ShouldThrow<InfrastructureException>();
        }

        [Test]
        public void AndSelectorThrowsWageRangeNotFoundException_ThenLetsExceptionBubble()
        {
            var request = new CreateApprenticeshipRequest
            {
                WageType = WageType.CustomWageFixed,
                FixedWage = _fixture.Create<decimal>(),
                WageUnit = _fixture.CreateAnyWageUnitOtherThanNotApplicable(),
                ExpectedStartDate = _fixture.CreateFutureDateTime(),
                HoursPerWeek = _fixture.Create<double>()
            };
            var context = GetValidationContextForProperty(request, req => req.FixedWage);

            _mockSelector
                .Setup(selector => selector.SelectHourlyRateAsync(It.IsAny<DateTime>()))
                .Throws<WageRangeNotFoundException>();

            var action = new Func<Task<ValidationResult>>(() => _validator.ValidateAsync(context));

            action.ShouldThrow<WageRangeNotFoundException>();
        }

        [Test]
        public async Task AndCalculatorThrowsException_ThenReturnsValidationResult()
        {
            var request = new CreateApprenticeshipRequest
            {
                WageType = WageType.CustomWageFixed,
                FixedWage = _fixture.Create<decimal>(),
                WageUnit = _fixture.CreateAnyWageUnitOtherThanNotApplicable(),
                ExpectedStartDate = _fixture.CreateFutureDateTime(),
                HoursPerWeek = _fixture.Create<double>()
            };
            var context = GetValidationContextForProperty(request, req => req.FixedWage);

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
        public async Task AndCheckingAllowedVersusAttemtpedMinWage(WageUnit wageUnit, decimal allowedMinimumHourlyWage, decimal attemptedFixedWage, bool expectedIsValid)
        {
            var request = new CreateApprenticeshipRequest
            {
                WageType = WageType.CustomWageFixed,
                WageUnit = wageUnit,
                FixedWage = attemptedFixedWage,
                HoursPerWeek = 36,
                ExpectedStartDate = _fixture.CreateFutureDateTime()
            };
            var context = GetValidationContextForProperty(request, req => req.FixedWage);

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
