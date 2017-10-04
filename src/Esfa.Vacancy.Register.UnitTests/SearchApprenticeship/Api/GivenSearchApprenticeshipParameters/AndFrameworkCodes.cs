using System.Collections.Generic;
using AutoMapper;
using Esfa.Vacancy.Api.Types;
using Esfa.Vacancy.Register.Api;
using Esfa.Vacancy.Register.Application.Queries.SearchApprenticeshipVacancies;
using NUnit.Framework;

namespace Esfa.Vacancy.Register.UnitTests.SearchApprenticeship.Api.GivenSearchApprenticeshipParameters
{
    [TestFixture]
    public class AndFrameworkCodes
    {
        [SetUp]
        public void Setup()
        {
            AutoMapperConfig.Configure();
        }

        [TestCaseSource(nameof(TestCases))]
        public IEnumerable<string> WhenValidInput_ThenReturnMappedRequest(string frameworkCodes)
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
                .SetName("One or more delimeted values are acceptable"),
            new TestCaseData("1,2, 23lkk")
                .Returns(new[]{"1","2", " 23lkk"})
                .SetName("Each value should be delimeted by comma"),
            new TestCaseData(" ")
                .Returns(null)
                .SetName("Empty string returns null"),
            new TestCaseData(null)
                .Returns(null)
                .SetName("Null returns null")
        };
    }
}