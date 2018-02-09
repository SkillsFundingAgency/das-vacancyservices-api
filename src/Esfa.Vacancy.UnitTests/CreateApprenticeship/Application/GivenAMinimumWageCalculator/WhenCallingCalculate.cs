using Esfa.Vacancy.Application.Commands.CreateApprenticeship;
using NUnit.Framework;

namespace Esfa.Vacancy.UnitTests.CreateApprenticeship.Application.GivenAMinimumWageCalculator
{
    [TestFixture]
    public class WhenCallingCalculate
    {
        [Test, Ignore("todo")]
        public void ThenUsesMinimumWageSelector()
        {
            var calculator = new MinimumWageCalculator();

            calculator.CalculateMinimumWage(new CreateApprenticeshipRequest());
        }
    }

    public class MinimumWageCalculator
    {
        public void CalculateMinimumWage(CreateApprenticeshipRequest request)
        {
            throw new System.NotImplementedException();
        }
    }
}