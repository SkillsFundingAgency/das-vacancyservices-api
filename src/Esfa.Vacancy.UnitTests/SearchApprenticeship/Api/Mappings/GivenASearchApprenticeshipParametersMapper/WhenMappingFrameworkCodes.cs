﻿using System.Collections.Generic;
using AutoMapper;
using Esfa.Vacancy.Api.Types;
using Esfa.Vacancy.Application.Queries.SearchApprenticeshipVacancies;
using Esfa.Vacancy.Register.Api;
using NUnit.Framework;

namespace Esfa.Vacancy.UnitTests.SearchApprenticeship.Api.Mappings.GivenASearchApprenticeshipParametersMapper
{
    [TestFixture]
    public class WhenMappingFrameworkCodes
    {
        private IMapper _mapper;

        [SetUp]
        public void Setup()
        {
            var config = AutoMapperConfig.Configure();
            _mapper = config.CreateMapper();
        }

        [TestCaseSource(nameof(TestCases))]
        public IEnumerable<string> AndMapExecutes(string frameworkCodes)
        {
            var parameters = new SearchApprenticeshipParameters {FrameworkLarsCodes = frameworkCodes};

            return _mapper
                .Map<SearchApprenticeshipVacanciesRequest>(parameters)
                .FrameworkLarsCodes;
        }

        private static List<TestCaseData> TestCases => new List<TestCaseData>
        {
            new TestCaseData("1")
                .Returns(new[]{"1"})
                .SetName("Then a single value is acceptable"),
            new TestCaseData("1,2,23lkk")
                .Returns(new[]{"1","2", "23lkk"})
                .SetName("Then a comma delimited array is split on comma"),
            new TestCaseData("1,2, 23lkk")
                .Returns(new[]{"1","2", "23lkk"})
                .SetName("Then a comma delimited array is split on comma and trims"),
            new TestCaseData(" ")
                .Returns(new List<string>())
                .SetName("Then an empty string returns empty list"),
            new TestCaseData(null)
                .Returns(new List<string>())
                .SetName("Then null returns empty list")
        };
    }
}