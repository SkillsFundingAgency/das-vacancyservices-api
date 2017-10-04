using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Esfa.Vacancy.Api.Types;
using Esfa.Vacancy.Register.Api.App_Start;
using Esfa.Vacancy.Register.Application.Queries.SearchApprenticeshipVacancies;
using FluentAssertions;
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
            new TestCaseData("1,2")
            .Returns(new[]{"1","2"})
            .SetName("Each number should be delimeted by comma")
        };
    }
}