using System.Collections.Generic;
using System.Linq;
using Esfa.Vacancy.Register.Application.Queries.SearchApprenticeshipVacancies;
using FluentAssertions;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace Esfa.Vacancy.Register.UnitTests.SearchApprenticeship.Application.GivenVacancySearchParametersMapper
{
    [TestFixture]
    public class AndFrameworkCodes
    {
        public static List<TestCaseData> TestCases => new List<TestCaseData>()
        {
            new TestCaseData(" 9 , 10 ").SetName("Then any leading and preceding spaces are trimed"),
            new TestCaseData("").SetName("Then empty list is allowed")
        };


        [TestCaseSource(nameof(TestCases))]
        public void WhenPopulatingFrameworkCodes(string frameworkCodes)
        {
            var expectedResult = frameworkCodes.Split(',').Select(x => x.Trim());
            var result = VacancySearchParametersMapper.Convert(new SearchApprenticeshipVacanciesRequest()
            { FrameworkCodes = frameworkCodes.Split(',').ToList() });

            result.FrameworkCodes.ShouldAllBeEquivalentTo(expectedResult);
        }
    }
}
