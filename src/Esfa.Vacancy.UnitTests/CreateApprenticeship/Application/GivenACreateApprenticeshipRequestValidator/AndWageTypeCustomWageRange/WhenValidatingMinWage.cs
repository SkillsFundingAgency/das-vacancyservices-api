using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Esfa.Vacancy.Application.Commands.CreateApprenticeship;
using Esfa.Vacancy.Application.Commands.CreateApprenticeship.Validators;
using Esfa.Vacancy.Application.Interfaces;
using Esfa.Vacancy.Domain.Validation;
using Esfa.Vacancy.Infrastructure.Exceptions;
using Esfa.Vacancy.Infrastructure.Services;
using Esfa.Vacancy.UnitTests.Extensions;
using FluentAssertions;
using FluentValidation.Results;
using Moq;
using NUnit.Framework;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.AutoMoq;
using SFA.DAS.NLog.Logger;
using SFA.DAS.VacancyServices.Wage;
using WageType = Esfa.Vacancy.Application.Commands.CreateApprenticeship.WageType;
using WageUnit = Esfa.Vacancy.Application.Commands.CreateApprenticeship.WageUnit;

namespace Esfa.Vacancy.UnitTests.CreateApprenticeship.Application.GivenACreateApprenticeshipRequestValidator.AndWageTypeCustomWageRange
{
    [TestFixture]
    public class WhenValidatingMinWage : CreateApprenticeshipRequestValidatorBase
    {
        private IFixture _fixture;
        private CreateApprenticeshipRequestValidator _validator;
        private Mock<IGetMinimumWagesService> _mockSelector;
        private Mock<IHourlyWageCalculator> _mockCalculator;
        private decimal _expectedAllowedMinimumWage;
        private decimal _expectedAttemptedMinimumWage;

        [SetUp]
        public void Setup()
        {
            _fixture = new Fixture().Customize(new AutoMoqCustomization());

            _expectedAllowedMinimumWage = _fixture.Create<decimal>();
            _expectedAttemptedMinimumWage = _fixture.Create<decimal>();

            _mockSelector = _fixture.Freeze<Mock<IGetMinimumWagesService>>();
            _mockSelector
                .Setup(selector => selector.GetApprenticeMinimumWageRate(It.IsAny<DateTime>()))
                .Returns(_expectedAllowedMinimumWage);

            _mockCalculator = _fixture.Freeze<Mock<IHourlyWageCalculator>>();
            _mockCalculator
                .Setup(calculator => calculator.Calculate(It.IsAny<decimal>(), It.IsAny<WageUnit>(), It.IsAny<decimal>()))
                .Returns(_expectedAttemptedMinimumWage);

            _validator = _fixture.Create<CreateApprenticeshipRequestValidator>();
        }

        [Test]
        public async Task AndNoValueThenIsInvalid()
        {
            var request = new CreateApprenticeshipRequest
            {
                WageType = WageType.CustomWageRange
            };

            var context = GetValidationContextForProperty(request, req => req.MinWage);

            var result = await _validator.ValidateAsync(context);

            result.IsValid.Should().Be(false);
            result.Errors.Count
                .Should().Be(1);
            result.Errors.First().ErrorCode
                .Should().Be(ErrorCodes.CreateApprenticeship.MinWage);
            result.Errors.First().ErrorMessage
                .Should().Be("'Min Wage' must not be empty.");

        }

        [Test]
        public async Task AndHasValueThenIsValid()
        {
            var request = new CreateApprenticeshipRequest
            {
                WageType = WageType.CustomWageRange,
                MinWage = 99.99m
            };

            var context = GetValidationContextForProperty(request, req => req.MinWage);

            var result = await _validator.ValidateAsync(context);

            result.IsValid.Should().Be(true);
        }

        [Test]
        public async Task AndValueNotMonetaryThenIsInvalid()
        {
            var request = new CreateApprenticeshipRequest
            {
                WageType = WageType.CustomWageRange,
                MinWage = 99.99999m
            };

            var context = GetValidationContextForProperty(request, req => req.MinWage);

            var result = await _validator.ValidateAsync(context);

            result.IsValid.Should().Be(false);
            result.Errors.First().ErrorCode
                .Should().Be(ErrorCodes.CreateApprenticeship.MinWage);
            result.Errors.First().ErrorMessage
                .Should().Be("'Min Wage' must be a monetary value.");
        }

        [Test]
        public async Task AndExpectedStartDateIsBeforeToday_ThenIsValid()
        {
            var request = new CreateApprenticeshipRequest
            {
                WageType = WageType.CustomWageRange,
                MinWage = _fixture.Create<decimal>(),
                WageUnit = _fixture.CreateAnyWageUnitOtherThanNotApplicable(),
                ExpectedStartDate = DateTime.Today.AddDays(-1),
                HoursPerWeek = _fixture.Create<double>()
            };
            var context = GetValidationContextForProperty(request, req => req.MinWage);

            var result = await _validator.ValidateAsync(context).ConfigureAwait(false);

            result.IsValid.Should().Be(true);
        }

        [Test]
        public async Task AndWageUnitIsNotApplicable_ThenIsValid()
        {
            var request = new CreateApprenticeshipRequest
            {
                WageType = WageType.CustomWageRange,
                MinWage = _fixture.Create<decimal>(),
                WageUnit = WageUnit.NotApplicable,
                ExpectedStartDate = _fixture.CreateFutureDateTime(),
                HoursPerWeek = _fixture.Create<double>()
            };
            var context = GetValidationContextForProperty(request, req => req.MinWage);

            var result = await _validator.ValidateAsync(context).ConfigureAwait(false);

            result.IsValid.Should().Be(true);
        }

        [Test]
        public async Task AndHoursPerWeekIsLessThanZero_ThenIsValid()
        {
            var request = new CreateApprenticeshipRequest
            {
                WageType = WageType.CustomWageRange,
                MinWage = _fixture.Create<decimal>(),
                WageUnit = _fixture.CreateAnyWageUnitOtherThanNotApplicable(),
                ExpectedStartDate = _fixture.CreateFutureDateTime(),
                HoursPerWeek = -9
            };
            var context = GetValidationContextForProperty(request, req => req.MinWage);

            var result = await _validator.ValidateAsync(context).ConfigureAwait(false);

            result.IsValid.Should().Be(true);
        }

        [Test]
        public async Task ThenUsesSelectorToGetAllowedMinimumWage()
        {
            var expectedStartDate = _fixture.CreateFutureDateTime();
            var request = new CreateApprenticeshipRequest
            {
                WageType = WageType.CustomWageRange,
                WageUnit = _fixture.CreateAnyWageUnitOtherThanNotApplicable(),
                ExpectedStartDate = expectedStartDate,
                MinWage = _fixture.Create<decimal>(),
                HoursPerWeek = _fixture.Create<double>()
            };
            var context = GetValidationContextForProperty(request, req => req.MinWage);

            await _validator.ValidateAsync(context);

            _mockSelector.Verify(selector => selector.GetApprenticeMinimumWageRate(expectedStartDate));
        }

        [Test]
        public async Task ThenUsesCalculatorToDetermineAttemptedMinimumWage()
        {
            var request = new CreateApprenticeshipRequest
            {
                WageType = WageType.CustomWageRange,
                WageUnit = _fixture.CreateAnyWageUnitOtherThanNotApplicable(),
                ExpectedStartDate = _fixture.CreateFutureDateTime(),
                MinWage = _fixture.Create<decimal>(),
                HoursPerWeek = _fixture.Create<double>()
            };
            var context = GetValidationContextForProperty(request, req => req.MinWage);

            await _validator.ValidateAsync(context);

            _mockCalculator.Verify(calculator => calculator.Calculate(request.MinWage.Value, request.WageUnit, (decimal)request.HoursPerWeek));
        }

        [Test]
        public void AndSelectorThrowsInfrastructureException_ThenLetsExceptionBubble()
        {
            var request = new CreateApprenticeshipRequest
            {
                WageType = WageType.CustomWageRange,
                MinWage = _fixture.Create<decimal>(),
                WageUnit = _fixture.CreateAnyWageUnitOtherThanNotApplicable(),
                ExpectedStartDate = _fixture.CreateFutureDateTime(),
                HoursPerWeek = _fixture.Create<double>()
            };
            var context = GetValidationContextForProperty(request, req => req.MinWage);

            _mockSelector
                .Setup(selector => selector.GetApprenticeMinimumWageRate(It.IsAny<DateTime>()))
                .Throws<InfrastructureException>();

            var action = new Func<Task<ValidationResult>>(() => _validator.ValidateAsync(context));

            action.ShouldThrow<InfrastructureException>();
        }

        [Test]
        public void AndSelectorThrowsWageRangeNotFoundException_ThenLetsExceptionBubble()
        {
            var request = new CreateApprenticeshipRequest
            {
                WageType = WageType.CustomWageRange,
                MinWage = _fixture.Create<decimal>(),
                WageUnit = _fixture.CreateAnyWageUnitOtherThanNotApplicable(),
                ExpectedStartDate = _fixture.CreateFutureDateTime(),
                HoursPerWeek = _fixture.Create<double>()
            };
            var context = GetValidationContextForProperty(request, req => req.MinWage);

            _mockSelector
                .Setup(selector => selector.GetApprenticeMinimumWageRate(It.IsAny<DateTime>()))
                .Throws<WageRangeNotFoundException>();

            var action = new Func<Task<ValidationResult>>(() => _validator.ValidateAsync(context));

            action.ShouldThrow<WageRangeNotFoundException>();
        }

        [Test]
        public async Task AndCalculatorThrowsException_ThenReturnsValidationResult()
        {
            var request = new CreateApprenticeshipRequest
            {
                WageType = WageType.CustomWageRange,
                MinWage = _fixture.Create<decimal>(),
                WageUnit = _fixture.CreateAnyWageUnitOtherThanNotApplicable(),
                ExpectedStartDate = _fixture.CreateFutureDateTime(),
                HoursPerWeek = _fixture.Create<double>()
            };
            var context = GetValidationContextForProperty(request, req => req.MinWage);

            _mockCalculator
                .Setup(calculator => calculator.Calculate(It.IsAny<decimal>(), It.IsAny<WageUnit>(), It.IsAny<decimal>()))
                .Throws<ArgumentOutOfRangeException>();

            var result = await _validator.ValidateAsync(context);

            result.IsValid.Should().Be(false);
            result.Errors.First().ErrorCode
                .Should().Be(ErrorCodes.CreateApprenticeship.MinWage);
        }
    }
}