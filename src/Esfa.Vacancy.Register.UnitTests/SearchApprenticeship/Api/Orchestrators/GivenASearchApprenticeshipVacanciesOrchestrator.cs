using System;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Routing;
using AutoMapper;
using Esfa.Vacancy.Api.Types;
using Esfa.Vacancy.Register.Api.Controllers;
using Esfa.Vacancy.Register.Api.Orchestrators;
using Esfa.Vacancy.Register.Application.Queries.SearchApprenticeshipVacancies;
using Esfa.Vacancy.Register.Domain.Validation;
using Esfa.Vacancy.Register.Infrastructure.Settings;
using FluentAssertions;
using FluentValidation;
using MediatR;
using Moq;
using NUnit.Framework;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.AutoMoq;

namespace Esfa.Vacancy.Register.UnitTests.SearchApprenticeship.Api.Orchestrators
{
    [TestFixture]
    public class GivenASearchApprenticeshipVacanciesOrchestrator
    {
        private SearchApprenticeshipVacanciesOrchestrator _orchestrator;
        private SearchApprenticeshipParameters _searchApprenticeshipParameters;
        private SearchResponse<ApprenticeshipSummary> _searchResponse;
        private Fixture _fixture;

        [SetUp]
        public void WhenCallingSearchApprenticeship()
        {
            _fixture = new Fixture();
            _fixture.Customize(new AutoMoqCustomization());

            _searchApprenticeshipParameters = _fixture.Create<SearchApprenticeshipParameters>();
            _searchResponse = _fixture.Create<SearchResponse<ApprenticeshipSummary>>();
            var searchApprenticeshipVacanciesRequest = _fixture.Create<SearchApprenticeshipVacanciesRequest>();
            var searchApprenticeshipVacanciesResponse = _fixture.Create<SearchApprenticeshipVacanciesResponse>();

            var mockMapper = new Mock<IMapper>();
            mockMapper
                .Setup(mapper => mapper.Map<SearchApprenticeshipVacanciesRequest>(_searchApprenticeshipParameters))
                .Returns(searchApprenticeshipVacanciesRequest);
            mockMapper
                .Setup(mapper => mapper.Map<SearchResponse<ApprenticeshipSummary>>(searchApprenticeshipVacanciesResponse))
                .Returns(_searchResponse);

            var mockMediator = new Mock<IMediator>();
            mockMediator
                .Setup(mediator => mediator.Send(searchApprenticeshipVacanciesRequest, CancellationToken.None))
                .ReturnsAsync(searchApprenticeshipVacanciesResponse);

            _orchestrator = new SearchApprenticeshipVacanciesOrchestrator(mockMediator.Object, mockMapper.Object);
        }

        [Test]
        public void AndParametersAreNull_ThenThrowsValidationException()
        {
            Func<Task> action = async () => { await _orchestrator.SearchApprenticeship(null, null); };

            action.ShouldThrow<ValidationException>().WithMessage($"Validation failed: \r\n -- {ErrorMessages.SearchApprenticeships.SearchApprenticeshipParametersIsNull}");
        }

        [Test]
        public async Task ThenMappedResponseFromMediatorIsReturned()
        {
            var response = await _orchestrator.SearchApprenticeship(_searchApprenticeshipParameters, new Mock<UrlHelper>().Object);

            response.Should().BeSameAs(_searchResponse);
        }

        [Test]
        public async Task ThenApiDetailsUrlIsPopulated()
        {
            var requestUrl = "http://localhost/api/v1/apprenticeships";
            var controller = new GetApprenticeshipVacancyController(new GetApprenticeshipVacancyOrchestrator(new Mock<IMediator>().Object, new Mock<IProvideSettings>().Object));
            controller.Request = new HttpRequestMessage() { RequestUri = new Uri(requestUrl) };
            controller.Configuration = new HttpConfiguration();
            controller.Configuration.MapHttpAttributeRoutes();
            controller.Configuration.EnsureInitialized();

            SearchResponse<ApprenticeshipSummary> response = await _orchestrator.SearchApprenticeship(_searchApprenticeshipParameters, controller.Url).ConfigureAwait(false);

            response.Results.First().ApiDetailUrl.Should().Be($"{requestUrl}/{_searchResponse.Results.First().VacancyReference}");
        }
    }
}
