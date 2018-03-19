using System;
using System.Linq;
using Esfa.Vacancy.Application.Commands.CreateApprenticeship;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Kernel;

namespace Esfa.Vacancy.UnitTests.Extensions
{
    public static class FixtureExtensions
    {
        public static WageUnit CreateAnyWageUnitOtherThanNotApplicable(this IFixture fixture)
        {
            return fixture.Create<Generator<WageUnit>>()
                .First(unit => unit != WageUnit.NotApplicable);
        }

        public static DateTime CreateFutureDateTime(this ISpecimenBuilder fixture)
        {
            return DateTime.Now.AddDays(1) + fixture.Create<TimeSpan>();
        }
    }
}