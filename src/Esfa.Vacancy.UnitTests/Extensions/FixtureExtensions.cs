using System.Linq;
using Esfa.Vacancy.Application.Commands.CreateApprenticeship;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Kernel;

namespace Esfa.Vacancy.UnitTests.Extensions
{
    public static class FixtureExtensions
    {
        private const double HoursPerWeekMinimumLength = 16;
        private const double HoursPerWeekMaximumLength = 48;

        public static WageUnit CreateAnyWageUnitOtherThanNotApplicable(this IFixture fixture)
        {
            return fixture.Create<Generator<WageUnit>>().First(unit => unit != WageUnit.NotApplicable);
        }

        public static double CreateValidHoursPerWeek(this ISpecimenBuilder fixture)
        {
            return fixture.Create<Generator<double>>()
                .First(d => d >= HoursPerWeekMinimumLength && d <= HoursPerWeekMaximumLength);
        }

        public static double CreateInValidHoursPerWeek(this ISpecimenBuilder fixture)
        {
            return fixture.Create<Generator<double>>()
                .First(d => d < HoursPerWeekMinimumLength || d > HoursPerWeekMaximumLength);
        }
    }
}