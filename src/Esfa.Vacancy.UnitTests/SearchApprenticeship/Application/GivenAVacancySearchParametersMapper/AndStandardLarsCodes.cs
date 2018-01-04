using System.Collections.Generic;
using System.Linq;
using Esfa.Vacancy.Application.Queries.SearchApprenticeshipVacancies;
using FluentAssertions;
using NUnit.Framework;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.AutoMoq;

namespace Esfa.Vacancy.UnitTests.SearchApprenticeship.Application.GivenAVacancySearchParametersMapper
{
    [TestFixture]
    public class AndStandardLarsCodes
    {
        private VacancySearchParametersMapper _mapper;

        [SetUp]
        public void SetUp()
        {
            var fixture = new Fixture().Customize(new AutoMoqCustomization());
            _mapper = fixture.Create<VacancySearchParametersMapper>();
        }

        private static List<TestCaseData> TestCases => new List<TestCaseData>()
        {
            new TestCaseData(" 9 , 10 ").SetName("Then any leading and preceding spaces are trimed"),
            new TestCaseData("").SetName("Then empty list is allowed")
        };
        
        [TestCaseSource(nameof(TestCases))]
        public void WhenPopulatingStandardCodes(string standardCodes)
        {
            var expectedResult = standardCodes.Split(',').Select(x => x.Trim());
            var result = _mapper.Convert(new SearchApprenticeshipVacanciesRequest()
            { StandardLarsCodes = standardCodes.Split(',').ToList() });

            result.StandardLarsCodes.ShouldAllBeEquivalentTo(expectedResult);
        }
    }
}
