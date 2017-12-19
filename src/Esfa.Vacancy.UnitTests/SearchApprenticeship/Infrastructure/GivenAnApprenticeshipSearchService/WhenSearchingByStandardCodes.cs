using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Elasticsearch.Net;
using Esfa.Vacancy.Application.Queries.SearchApprenticeshipVacancies;
using Esfa.Vacancy.Domain.Entities;
using Esfa.Vacancy.Infrastructure.Exceptions;
using Esfa.Vacancy.Infrastructure.Services;
using Esfa.Vacancy.Infrastructure.Settings;
using FluentAssertions;
using Moq;
using Nest;
using NUnit.Framework;
using SFA.DAS.NLog.Logger;

namespace Esfa.Vacancy.UnitTests.SearchApprenticeship.Infrastructure.GivenAnApprenticeshipSearchService
{
    [TestFixture]
    public class WhenSearchingByStandardCodes
    {
        private List<ApprenticeshipSummary> _apprenticeshipSummaries;
        private SearchApprenticeshipVacanciesResponse _actualResponse;
        private Mock<IElasticClient> _mockElasticClient;
        private ApprenticeshipSearchService _apprenticeshipSearchService;
        private Mock<IElasticsearchResponse> _connectionStatus;
        private int _expectedTotal;
        private int _expectedCurrentPage;
        private double _expectedTotalPages;
        private SortBy _expectedSortBy;

        [SetUp]
        public async Task Setup()
        {
            var pageSize = 68;
            _expectedTotal = 3245458;
            _expectedCurrentPage = 37987;
            _expectedSortBy = SortBy.Distance;
            var vacancySearchParameters = new VacancySearchParameters
            {
                PageSize = 68,
                PageNumber = _expectedCurrentPage,
                SortBy = _expectedSortBy
            };
            _expectedTotalPages = Math.Ceiling((double) _expectedTotal / pageSize);

            _apprenticeshipSummaries = new List<ApprenticeshipSummary>
            {
                new ApprenticeshipSummary {Id = 234243},
                new ApprenticeshipSummary {Id = 226453453},
                new ApprenticeshipSummary {Id = 9089853}
            };

            _connectionStatus = new Mock<IElasticsearchResponse>();
            _connectionStatus
                .Setup(response => response.Success)
                .Returns(true);

            var searchResponse = new Mock<ISearchResponse<ApprenticeshipSummary>>();
            searchResponse
                .Setup(response => response.Total)
                .Returns(_expectedTotal);
            searchResponse
                .Setup(response => response.Documents)
                .Returns(_apprenticeshipSummaries);
            searchResponse
                .Setup(response => response.ConnectionStatus)
                .Returns(_connectionStatus.Object);

            _mockElasticClient = new Mock<IElasticClient>();
            _mockElasticClient
                .Setup(client => client.SearchAsync(It.IsAny<Func<SearchDescriptor<ApprenticeshipSummary>, SearchDescriptor<ApprenticeshipSummary>>>()))
                .ReturnsAsync(searchResponse.Object);

            _apprenticeshipSearchService = new ApprenticeshipSearchService(new Mock<IProvideSettings>().Object, new Mock<ILog>().Object, _mockElasticClient.Object);

            _actualResponse = await _apprenticeshipSearchService.SearchApprenticeshipVacanciesAsync(vacancySearchParameters);
        }

        [Test]
        public void AndNoExceptionThrownByElasticClient_ThenReturnsResult()
        {
            _actualResponse.ApprenticeshipSummaries.ShouldAllBeEquivalentTo(_apprenticeshipSummaries);
        }

        [Test]
        public void AndWebExceptionThrownByElasticClient_ThenThrowsInfrastructureException()
        {
            _mockElasticClient
                .Setup(client => client.SearchAsync(It.IsAny<Func<SearchDescriptor<ApprenticeshipSummary>, SearchDescriptor<ApprenticeshipSummary>>>()))
                .Throws<WebException>();

            var ex = Assert.ThrowsAsync<InfrastructureException>(() => _apprenticeshipSearchService.SearchApprenticeshipVacanciesAsync(new VacancySearchParameters()));
            Assert.That(ex.InnerException, Is.TypeOf<WebException>());
        }

        [Test]
        public void AndConnectionStatusNotSuccess_ThenThrowsInfrastructureException()
        {
            _connectionStatus
                .Setup(response => response.Success)
                .Returns(false);

            Assert.ThrowsAsync<InfrastructureException>(() => _apprenticeshipSearchService.SearchApprenticeshipVacanciesAsync(new VacancySearchParameters()));
        }

        [Test]
        public void ThenTotalMatchedIsCorrect()
        {
            _actualResponse.TotalMatched.Should().Be(_expectedTotal);
        }

        [Test]
        public void ThenTotalReturnedIsCorrect()
        {
            _actualResponse.TotalReturned.Should().Be(_apprenticeshipSummaries.Count);
        }

        [Test]
        public void ThenCurrentPageIsCorrect()
        {
            _actualResponse.CurrentPage.Should().Be(_expectedCurrentPage);
        }

        [Test]
        public void ThenTotalPagesIsCorrect()
        {
            _actualResponse.TotalPages.Should().Be(_expectedTotalPages);
        }

        [Test]
        public void ThenApprenticeshipSummariesIsCorrect()
        {
            _actualResponse.ApprenticeshipSummaries.ShouldAllBeEquivalentTo(_apprenticeshipSummaries);
        }

        [Test]
        public void ThenSortByIsCorrect()
        {
            _actualResponse.SortBy.Should().Be(_expectedSortBy);
        }
    }
}