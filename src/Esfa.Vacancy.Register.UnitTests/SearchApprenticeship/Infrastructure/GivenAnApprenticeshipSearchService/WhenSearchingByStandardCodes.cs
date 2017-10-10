using System;
using System.Collections.Generic;
using System.Net;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Elasticsearch.Net;
using Esfa.Vacancy.Register.Application.Queries.SearchApprenticeshipVacancies;
using Esfa.Vacancy.Register.Domain.Entities;
using Esfa.Vacancy.Register.Infrastructure.Exceptions;
using Esfa.Vacancy.Register.Infrastructure.Services;
using Esfa.Vacancy.Register.Infrastructure.Settings;
using FluentAssertions;
using Moq;
using Nest;
using NUnit.Framework;
using SFA.DAS.NLog.Logger;

namespace Esfa.Vacancy.Register.UnitTests.SearchApprenticeship.Infrastructure.GivenAnApprenticeshipSearchService
{
    [TestFixture]
    public class WhenSearchingByStandardCodes
    {
        private List<ApprenticeshipSummary> _apprenticeshipSummaries;
        private SearchApprenticeshipVacanciesResponse _actualResponse;
        private Mock<IElasticClient> _mockElasticClient;
        private ApprenticeshipSearchService _apprenticeshipSearchService;

        [SetUp]
        public async Task Setup()
        {
            _apprenticeshipSummaries = new List<ApprenticeshipSummary>
            {
                new ApprenticeshipSummary {Id = 234243},
                new ApprenticeshipSummary {Id = 226453453},
                new ApprenticeshipSummary {Id = 9089853}
            };

            var connectionStatus = new Mock<IElasticsearchResponse>();
            connectionStatus
                .Setup(response => response.Success)
                .Returns(true);

            var searchResponse = new Mock<ISearchResponse<ApprenticeshipSummary>>();
            searchResponse
                .Setup(response => response.Total)
                .Returns(3);
            searchResponse
                .Setup(response => response.Documents)
                .Returns(_apprenticeshipSummaries);
            searchResponse
                .Setup(response => response.ConnectionStatus)
                .Returns(connectionStatus.Object);

            _mockElasticClient = new Mock<IElasticClient>();
            _mockElasticClient
                .Setup(client => client.SearchAsync<ApprenticeshipSummary>(It.IsAny<Func<SearchDescriptor<ApprenticeshipSummary>, SearchDescriptor<ApprenticeshipSummary>>>()))
                .ReturnsAsync(searchResponse.Object);

            _apprenticeshipSearchService = new ApprenticeshipSearchService(new Mock<IProvideSettings>().Object, new Mock<ILog>().Object, _mockElasticClient.Object);
            
            _actualResponse = await _apprenticeshipSearchService.SearchApprenticeshipVacanciesAsync(new VacancySearchParameters());
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

        // AndConnectionStatusNotSuccess_ThenThrowsInfrastructureException
        // AndConnectionStatusSuccess_ThenReturnsResult
    }
}