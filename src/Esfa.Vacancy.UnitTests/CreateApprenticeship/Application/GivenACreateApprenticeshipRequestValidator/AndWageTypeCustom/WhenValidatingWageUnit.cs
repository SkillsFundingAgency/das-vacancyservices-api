using System;
using System.Linq;
using System.Threading.Tasks;
using Esfa.Vacancy.Application.Commands.CreateApprenticeship;
using Esfa.Vacancy.Application.Commands.CreateApprenticeship.Validators;
using Esfa.Vacancy.Domain.Validation;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.AutoMoq;

namespace Esfa.Vacancy.UnitTests.CreateApprenticeship.Application.GivenACreateApprenticeshipRequestValidator.AndWageTypeCustom
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
        public void AndAttemptedIsLowerThanAllowed_ThenInvalid() { }

        [Test]
        public void AndAttemptedIsEqualToAllowed_ThenValid() { }

        [Test]
        public void AndAttemptedIsHigherThanAllowed_ThenValid() { }

        [Test]
        public void AndExceptionOccurs_ThenInvalid() { }


        /*private static List<TestCaseData> WageCalculationTestCases => new List<TestCaseData>
        {
            new TestCaseData(WageUnit.Weekly, 131.24m, 37.5, false).SetName("And weekly wage is less than 3.50 per hour Then is invalid"),
            new TestCaseData(WageUnit.Weekly, 131.25m, 37.5, true).SetName("And weekly wage is exactly 3.50 per hour Then is valid"),
            new TestCaseData(WageUnit.Weekly, 131.26m, 37.5, true).SetName("And weekly wage is greater than 3.50 per hour Then is valid"),
            //todo: same for monthly and yearly
        };

        [TestCaseSource(nameof(WageCalculationTestCases))]
        public async Task AndCalculatedWageLessThanApprenticeshipMinimumWage(WageUnit wageUnit, decimal minWage, double hoursPerWeek, bool expectedIsValid)
        {
            var fixture = new Fixture().Customize(new AutoMoqCustomization());
            var request = new CreateApprenticeshipRequest
            {
                WageType = WageType.Custom,
                WageUnit = wageUnit,
                MinWage = minWage,
                HoursPerWeek = hoursPerWeek
            };

            var context = GetValidationContextForProperty(request, req => req.WageUnit);

            var validator = fixture.Create<CreateApprenticeshipRequestValidator>();

            var result = await validator.ValidateAsync(context);

            result.IsValid.Should().Be(expectedIsValid);
            if (!result.IsValid)
            {
                result.Errors.First().ErrorCode.Should().Be(ErrorCodes.CreateApprenticeship.WageUnit);
            }
        }*/
    }
}