using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Elasticsearch.Net;
using Esfa.Vacancy.Application.Queries.SearchApprenticeshipVacancies;
using Esfa.Vacancy.Domain.Entities;
using Esfa.Vacancy.Infrastructure.Exceptions;
using Esfa.Vacancy.Infrastructure.Services;
using FluentAssertions;
using Moq;
using Nest;
using NUnit.Framework;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.AutoMoq;

namespace Esfa.Vacancy.UnitTests.SearchApprenticeship.Infrastructure.GivenAnApprenticeshipSearchService
{
    [TestFixture]
    public class WhenSearching
    {
        private VacancySearchParameters _vacancySearchParameters;
        private List<ApprenticeshipSummary> _apprenticeshipSummaries;
        private SearchApprenticeshipVacanciesResponse _actualResponse;
        private ApprenticeshipSearchService _apprenticeshipSearchService;
        private int _expectedTotal;
        private int _expectedCurrentPage;
        private double _expectedTotalPages;
        private SortBy _expectedSortBy;
        private Mock<ISearchResponse<ApprenticeshipSummary>> _mockElasticsearchResponse;
        private Mock<IElasticClient> _mockElasticClient;
        private Mock<IGeoSearchResultDistanceSetter> _mockDistanceSetter;
        private Mock<ISearchResponse<ApprenticeshipSummary>> _mockSearchResponse;

        [SetUp]
        public async Task Setup()
        {
            var fixture = new Fixture().Customize(new AutoMoqCustomization());

            var pageSize = fixture.Create<int>();
            _expectedTotal = fixture.Create<int>();
            _expectedCurrentPage = fixture.Create<int>();
            _expectedSortBy = SortBy.Distance;
            _vacancySearchParameters = new VacancySearchParameters
            {
                PageSize = pageSize,
                PageNumber = _expectedCurrentPage,
                SortBy = _expectedSortBy,
                Latitude = 32,
                Longitude = 2,
                DistanceInMiles = 342
            };
            _expectedTotalPages = Math.Ceiling((double) _expectedTotal / pageSize);

            _apprenticeshipSummaries = fixture.Create<List<ApprenticeshipSummary>>();

            _mockElasticsearchResponse = fixture.Freeze<Mock<ISearchResponse<ApprenticeshipSummary>>>();
            _mockElasticsearchResponse
                .Setup(response => response.IsValid)
                .Returns(true);

            _mockSearchResponse = fixture.Freeze<Mock<ISearchResponse<ApprenticeshipSummary>>>();
            _mockSearchResponse
                .Setup(response => response.Total)
                .Returns(_expectedTotal);
            _mockSearchResponse
                .Setup(response => response.Documents)
                .Returns(_apprenticeshipSummaries);
            _mockSearchResponse
                .Setup(response => response.IsValid)
                .Returns(true);

            _mockElasticClient = fixture.Freeze<Mock<IElasticClient>>();
            _mockElasticClient
                .Setup(client => client.SearchAsync(It.IsAny<Func<SearchDescriptor<ApprenticeshipSummary>, ISearchRequest>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(_mockSearchResponse.Object);

            _mockDistanceSetter = fixture.Freeze<Mock<IGeoSearchResultDistanceSetter>>();

            _apprenticeshipSearchService = fixture.Create<ApprenticeshipSearchService>();

            _actualResponse = await _apprenticeshipSearchService.SearchApprenticeshipVacanciesAsync(_vacancySearchParameters);
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
                .Setup(client => client.SearchAsync(It.IsAny<Func<SearchDescriptor<ApprenticeshipSummary>, ISearchRequest>>(), It.IsAny<CancellationToken>()))
                .Throws<WebException>();

            var ex = Assert.ThrowsAsync<InfrastructureException>(() => _apprenticeshipSearchService.SearchApprenticeshipVacanciesAsync(new VacancySearchParameters()));
            Assert.That(ex.InnerException, Is.TypeOf<WebException>());
        }

        [Test]
        public void AndConnectionStatusNotSuccess_ThenThrowsInfrastructureException()
        {
            _mockElasticsearchResponse
                .Setup(response => response.IsValid)
                .Returns(false);

            Assert.ThrowsAsync<InfrastructureException>(() => _apprenticeshipSearchService.SearchApprenticeshipVacanciesAsync(new VacancySearchParameters()));
        }

        [Test]
        public void AndIsGeoSearch_ThenDistanceSetterIsCalled()
        {
            _mockDistanceSetter.Verify(setter => setter.SetDistance(_vacancySearchParameters, _mockSearchResponse.Object), Times.Once);
        }

        [Test]
        public async Task AndIsNotGeoSearch_ThenDistanceSetterIsNotCalled()
        {
            _mockDistanceSetter.ResetCalls();
            await _apprenticeshipSearchService.SearchApprenticeshipVacanciesAsync(new VacancySearchParameters());
            _mockDistanceSetter.Verify(setter => setter.SetDistance(It.IsAny<VacancySearchParameters>(), It.IsAny<ISearchResponse<ApprenticeshipSummary>>()), Times.Never);
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