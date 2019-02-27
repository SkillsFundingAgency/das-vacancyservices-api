using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Esfa.Vacancy.Application.Queries.SearchApprenticeshipVacancies;
using Esfa.Vacancy.Domain.Entities;
using Esfa.Vacancy.Infrastructure.Exceptions;
using Esfa.Vacancy.Infrastructure.Services;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.AutoMoq;
using SFA.DAS.VacancyServices.Search;
using SFA.DAS.VacancyServices.Search.Entities;
using SFA.DAS.VacancyServices.Search.Requests;
using SFA.DAS.VacancyServices.Search.Responses;

namespace Esfa.Vacancy.UnitTests.SearchApprenticeship.Infrastructure.GivenAnApprenticeshipSearchService
{
    [TestFixture]
    public class WhenSearching
    {
        private VacancySearchParameters _vacancySearchParameters;
        private List<ApprenticeshipSearchResult> _apprenticeshipResults;
        private SearchApprenticeshipVacanciesResponse _actualResponse;
        private ApprenticeshipSearchService _apprenticeshipSearchService;
        private int _expectedTotal;
        private int _expectedCurrentPage;
        private double _expectedTotalPages;
        private SortBy _expectedSortBy;
        private Mock<IApprenticeshipSearchClient> _mockSearchClient;
        private ApprenticeshipSearchResponse _apprenticeshipSearchClientResponse;

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

            _apprenticeshipResults = fixture.Create<List<ApprenticeshipSearchResult>>();

            _mockSearchClient = fixture.Freeze<Mock<IApprenticeshipSearchClient>>();

            _mockSearchClient.Setup(c => c.Search(It.IsAny<ApprenticeshipSearchRequestParameters>()))
                .Returns((ApprenticeshipSearchRequestParameters r) => new ApprenticeshipSearchResponse(
                    _expectedTotal,
                    _apprenticeshipResults,
                    Enumerable.Empty<AggregationResult>(),
                    r));

            _apprenticeshipSearchService = fixture.Create<ApprenticeshipSearchService>();

            _actualResponse = await _apprenticeshipSearchService.SearchApprenticeshipVacanciesAsync(_vacancySearchParameters);
        }

        [Test]
        public void AndNoExceptionThrownByElasticClient_ThenReturnsResult()
        {
            _actualResponse.ApprenticeshipSummaries.Count().Should().Be(_apprenticeshipResults.Count);
        }

        [Test]
        public void AndExceptionThrownBySearchClient_ThenThrowsInfrastructureException()
        {
            _mockSearchClient
                .Setup(client => client.Search(It.IsAny<ApprenticeshipSearchRequestParameters>()))
                .Throws<Exception>();

            var ex = Assert.ThrowsAsync<InfrastructureException>(() => _apprenticeshipSearchService.SearchApprenticeshipVacanciesAsync(new VacancySearchParameters()));
            Assert.That(ex.InnerException, Is.TypeOf<Exception>());
        }

        [Test]
        public void ThenTotalMatchedIsCorrect()
        {
            _actualResponse.TotalMatched.Should().Be(_expectedTotal);
        }

        [Test]
        public void ThenTotalReturnedIsCorrect()
        {
            _actualResponse.TotalReturned.Should().Be(_apprenticeshipResults.Count);
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
            var actualApprenticeshipSummaries = _actualResponse.ApprenticeshipSummaries.ToList();

            actualApprenticeshipSummaries.Count.Should().Be(_apprenticeshipResults.Count);

            for (var i = 0; i < actualApprenticeshipSummaries.Count; i++)
            {
                var actual = actualApprenticeshipSummaries[i];
                var expected = _apprenticeshipResults[i];

                actual.DistanceInMiles.Should().Be(expected.Distance);
                actual.AnonymousEmployerName.Should().Be(expected.AnonymousEmployerName);
                actual.ApprenticeshipLevel.Should().Be(expected.ApprenticeshipLevel.ToString());
                actual.Category.Should().Be(expected.Category);
                actual.CategoryCode.Should().Be(expected.CategoryCode);
                actual.ClosingDate.Should().Be(expected.ClosingDate);
                actual.Description.Should().Be(expected.Description);
                actual.EmployerName.Should().Be(expected.EmployerName);
                actual.FrameworkLarsCode.Should().Be(expected.FrameworkLarsCode);
                actual.HoursPerWeek.Should().Be(expected.HoursPerWeek);
                actual.Id.Should().Be(expected.Id);
                actual.IsDisabilityConfident.Should().Be(expected.IsDisabilityConfident);
                actual.IsEmployerAnonymous.Should().Be(expected.IsEmployerAnonymous);
                actual.IsPositiveAboutDisability.Should().Be(expected.IsPositiveAboutDisability);
                actual.Location.Lat.Should().Be(expected.Location.lat);
                actual.Location.Lon.Should().Be(expected.Location.lon);
                actual.NumberOfPositions.Should().Be(expected.NumberOfPositions);
                actual.PostedDate.Should().Be(expected.PostedDate);
                actual.ProviderName.Should().Be(expected.ProviderName);
                actual.StandardLarsCode.Should().Be(expected.StandardLarsCode);
                actual.StartDate.Should().Be(expected.StartDate);
                actual.SubCategory.Should().Be(expected.SubCategory);
                actual.SubCategoryCode.Should().Be(expected.SubCategoryCode);
                actual.Title.Should().Be(expected.Title);
                actual.VacancyLocationType.Should().Be(expected.VacancyLocationType.ToString());
                actual.VacancyReference.Should().Be(expected.VacancyReference);
                actual.WageAmount.Should().Be(expected.WageAmount);
                actual.WageAmountLowerBound.Should().Be(expected.WageAmountLowerBound);
                actual.WageAmountUpperBound.Should().Be(expected.WageAmountUpperBound);
                actual.WageText.Should().Be(expected.WageText);
                actual.WageType.Should().Be(expected.WageType);
                actual.WageUnit.Should().Be(expected.WageUnit);
                actual.WorkingWeek.Should().Be(expected.WorkingWeek);
                actual.ProviderUkprn.Should().Be(expected.ProviderUkprn);
            }
        }

        [Test]
        public void ThenSortByIsCorrect()
        {
            _actualResponse.SortBy.Should().Be(_expectedSortBy);
        }
    }
}