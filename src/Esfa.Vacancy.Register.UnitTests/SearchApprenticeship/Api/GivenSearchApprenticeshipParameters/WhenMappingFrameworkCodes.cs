using System.Collections.Generic;
using AutoMapper;
using Esfa.Vacancy.Api.Types;
using Esfa.Vacancy.Register.Api;
using Esfa.Vacancy.Register.Application.Queries.SearchApprenticeshipVacancies;
using NUnit.Framework;

namespace Esfa.Vacancy.Register.UnitTests.SearchApprenticeship.Api.GivenSearchApprenticeshipParameters
{
    [TestFixture]
    public class WhenMappingFrameworkCodes
    {
        [SetUp]
        public void Setup()
        {
            AutoMapperConfig.Configure();
        }

        [TestCaseSource(nameof(TestCases))]
        public IEnumerable<string> AndMapExecutes(string frameworkCodes)
        {
            var parameters = new SearchApprenticeshipParameters {FrameworkCodes = frameworkCodes};

            return Mapper
                .Map<SearchApprenticeshipVacanciesRequest>(parameters)
                .FrameworkCodes;
        }

        private static List<TestCaseData> TestCases => new List<TestCaseData>
        {
            new TestCaseData("1")
                .Returns(new[]{"1"})
                .SetName("Then a single value is acceptable"),
            new TestCaseData("1,2, 23lkk")
                .Returns(new[]{"1","2", " 23lkk"})
                .SetName("Then a comma delimited array is split on comma"),
            new TestCaseData(" ")
                .Returns(null)
                .SetName("Then an empty string returns null"),
            new TestCaseData(null)
                .Returns(null)
                .SetName("Then null returns null")
        };
    }
}