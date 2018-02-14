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

namespace Esfa.Vacancy.UnitTests.CreateApprenticeship.Application.GivenACreateApprenticeshipRequestValidator.AndWageTypeCustom
{
    /*[TestFixture, Ignore("ioc failing on deployed envts")]
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
                .Setup(calculator => calculator.CalculateMinimumWage(It.IsAny<CreateApprenticeshipRequest>()))
                .Returns(_expectedAttemptedMinimumWage);

            _validator = _fixture.Create<CreateApprenticeshipRequestValidator>();
        }

        [Test]
        public async Task AndIsNotApplicableThenIsInvalid()
        {
            var request = new CreateApprenticeshipRequest
            {
                WageType = WageType.Custom,
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
                WageType = WageType.Custom,
                WageUnit = _fixture.Create<Generator<WageUnit>>().First(unit => unit != WageUnit.NotApplicable),
                ExpectedStartDate = expectedStartDate
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
                WageType = WageType.Custom,
                WageUnit = _fixture.Create<Generator<WageUnit>>().First(unit => unit != WageUnit.NotApplicable),
                ExpectedStartDate = expectedStartDate
            };
            var context = GetValidationContextForProperty(request, req => req.WageUnit);

            await _validator.ValidateAsync(context);

            _mockCalculator.Verify(calculator => calculator.CalculateMinimumWage(request));
        }

        [Test]
        public void AndSelectorThrowsInfrastructureException_ThenLetsExceptionBubble()
        {
            var expectedStartDate = _fixture.Create<DateTime>();
            var request = new CreateApprenticeshipRequest
            {
                WageType = WageType.Custom,
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
                WageType = WageType.Custom,
                WageUnit = _fixture.Create<Generator<WageUnit>>().First(unit => unit != WageUnit.NotApplicable),
                ExpectedStartDate = expectedStartDate
            };
            var context = GetValidationContextForProperty(request, req => req.WageUnit);

            _mockCalculator
                .Setup(calculator => calculator.CalculateMinimumWage(It.IsAny<CreateApprenticeshipRequest>()))
                .Throws<ArgumentOutOfRangeException>();

            var result = await _validator.ValidateAsync(context);

            result.IsValid.Should().Be(false);
            result.Errors.First().ErrorCode
                .Should().Be(ErrorCodes.CreateApprenticeship.MinWage);
        }

        private static List<TestCaseData> TestCases => new List<TestCaseData>
        {
            new TestCaseData(131.25m, 131.24m, false).SetName("And attempted is less than allowed Then is invalid"),
            new TestCaseData(131.25m, 131.25m, true).SetName("And attempted is same as allowed Then is valid"),
            new TestCaseData(131.25m, 131.26m, true).SetName("And attempted is greater than allowed Then is valid")
        };

        [TestCaseSource(nameof(TestCases))]
        public async Task AndCheckingAllowedVersusAttemtpedMinWage(decimal allowedMinWage, decimal attemptedMinWage, bool expectedIsValid)
        {
            var request = new CreateApprenticeshipRequest
            {
                WageType = WageType.Custom,
                WageUnit = WageUnit.Annually,
                MinWage = _fixture.Create<decimal>(),
                ExpectedStartDate = _fixture.Create<DateTime>()
            };
            var context = GetValidationContextForProperty(request, req => req.WageUnit);

            _mockSelector = _fixture.Freeze<Mock<IMinimumWageSelector>>();
            _mockSelector
                .Setup(selector => selector.SelectHourlyRateAsync(It.IsAny<DateTime>()))
                .ReturnsAsync(allowedMinWage);

            _mockCalculator = _fixture.Freeze<Mock<IMinimumWageCalculator>>();
            _mockCalculator
                .Setup(calculator => calculator.CalculateMinimumWage(It.IsAny<CreateApprenticeshipRequest>()))
                .Returns(attemptedMinWage);

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
    }*/
}