using Esfa.Vacancy.Application.Queries.SearchApprenticeshipVacancies;
using FluentAssertions;
using NUnit.Framework;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.AutoMoq;

namespace Esfa.Vacancy.UnitTests.SearchApprenticeship.Application.GivenAVacancySearchParametersMapper
{
    [TestFixture]
    public class AndProviderUkprn
    {
        private VacancySearchParametersMapper _mapper;

        [SetUp]
        public void SetUp()
        {
            var fixture = new Fixture().Customize(new AutoMoqCustomization());
            _mapper = fixture.Create<VacancySearchParametersMapper>();
        }

        [Test]
        public void WhenMappingProviderUkprn_ThenMappedToSearchParams()
        {
            var ukprn = 88888888;
            var request = new SearchApprenticeshipVacanciesRequest
            {
                Ukprn = ukprn
            };

            var result = _mapper.Convert(request);

            result.Ukprn.Should().Be(ukprn);
        }

       
    }
}