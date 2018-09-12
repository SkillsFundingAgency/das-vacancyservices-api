using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Esfa.Vacancy.Api.Core.Validation;
using Esfa.Vacancy.Api.Types;
using Esfa.Vacancy.Application.Queries.SearchApprenticeshipVacancies;
using Esfa.Vacancy.Domain.Constants;
using Esfa.Vacancy.Domain.Interfaces;
using Esfa.Vacancy.Domain.Validation;
using Esfa.Vacancy.Register.Api.Orchestrators;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Moq;
using NUnit.Framework;
using Ploeh.AutoFixture;

namespace Esfa.Vacancy.UnitTests.SearchApprenticeship.Api.Orchestrators
{
    [TestFixture]
    public class GivenASearchApprenticeshipVacanciesOrchestrator
    {
        private const string FAABaseUrl = "https://findapprentice.com/apprenticeship/reference";
        private const string ApiBaseUrl = "http://localhost/api/v1/apprenticeships";
        private SearchApprenticeshipVacanciesOrchestrator _orchestrator;
        private SearchApprenticeshipParameters _searchApprenticeshipParameters;
        private SearchResponse<ApprenticeshipSummary> _searchResponse;
        private Fixture _fixture;
        private Mock<IValidationExceptionBuilder> _mockValidationExceptionBuilder;
        private string _expectedErrorMessage;

        [SetUp]
        public void WhenCallingSearchApprenticeship()
        {
            _fixture = new Fixture();

            _searchApprenticeshipParameters = _fixture.Create<SearchApprenticeshipParameters>();
            _searchResponse = _fixture.Create<SearchResponse<ApprenticeshipSummary>>();
            var searchApprenticeshipVacanciesRequest = _fixture.Create<SearchApprenticeshipVacanciesRequest>();
            var searchApprenticeshipVacanciesResponse = _fixture.Create<SearchApprenticeshipVacanciesResponse>();
            _expectedErrorMessage = _fixture.Create<string>();

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

            var mockSettings = new Mock<IProvideSettings>();
            mockSettings
                .Setup(x => x.GetSetting(ApplicationSettingKeys.LiveApprenticeshipVacancyBaseUrlKey))
                .Returns(FAABaseUrl);

            _mockValidationExceptionBuilder = _fixture.Freeze<Mock<IValidationExceptionBuilder>>(composer => composer.Do(mock => mock
                .Setup(builder => builder.Build(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(new ValidationException(new List<ValidationFailure>
                {
                    new ValidationFailure("", _expectedErrorMessage)
                }))
            ));

            _orchestrator = new SearchApprenticeshipVacanciesOrchestrator(mockMediator.Object, mockMapper.Object, mockSettings.Object, _mockValidationExceptionBuilder.Object);
        }

        [Test]
        public void AndParametersAreNull_ThenThrowsValidationException()
        {
            Func<Task> action = async () =>
            {
                await _orchestrator.SearchApprenticeship(null);
            };

            action.ShouldThrow<ValidationException>()
                .WithMessage($"Validation failed: \r\n -- {_expectedErrorMessage}");

            _mockValidationExceptionBuilder.Verify(builder =>
                builder.Build(
                    ErrorCodes.SearchApprenticeships.InvalidRequest,
                    Vacancy.Domain.Validation.ErrorMessages.SearchApprenticeships.SearchApprenticeshipParametersIsNull,
                    It.IsAny<string>()), Times.Once);
        }

        [Test]
        public async Task ThenMappedResponseFromMediatorIsReturned()
        {
            var response = await _orchestrator.SearchApprenticeship(_searchApprenticeshipParameters);

            response.Should().BeSameAs(_searchResponse);
        }

        [Test]
        public async Task ThenVacancyUrlShouldBePopulated()
        {
            SearchResponse<ApprenticeshipSummary> response = await _orchestrator
                .SearchApprenticeship(_searchApprenticeshipParameters)
                .ConfigureAwait(false);

            response.Results.First().VacancyUrl.Should().Be($"{FAABaseUrl}/{_searchResponse.Results.First().VacancyReference}");
        }
    }
}
