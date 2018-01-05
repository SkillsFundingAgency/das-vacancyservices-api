using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Esfa.Vacancy.Application.Interfaces;
using Esfa.Vacancy.Application.Queries.SearchApprenticeshipVacancies;
using Esfa.Vacancy.Domain.Entities;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Moq;
using NUnit.Framework;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.AutoMoq;

namespace Esfa.Vacancy.UnitTests.SearchApprenticeship.Application.GivenASearchApprenticeshipVacanciesQueryHandler
{
    [TestFixture]
    public class WhenCallingHandle
    {
        private SearchApprenticeshipVacanciesQueryHandler _handler;
        private SearchApprenticeshipVacanciesResponse _expectedResponse;
        private Mock<IValidator<SearchApprenticeshipVacanciesRequest>> _validatorMock;
        private SearchApprenticeshipVacanciesRequest _validRequest;
        private VacancySearchParameters _expectedParameters;
        private SearchApprenticeshipVacanciesResponse _response;
        private Mock<IVacancySearchParametersMapper> _mockMapper;
        private Mock<IApprenticeshipSearchService> _mockSearchService;

        [SetUp]
        public async Task Setup()
        {
            var fixture = new Fixture().Customize(new AutoMoqCustomization());
            _expectedResponse = new SearchApprenticeshipVacanciesResponse();
            _validRequest = new SearchApprenticeshipVacanciesRequest();
            _expectedParameters = new VacancySearchParameters();

            _validatorMock = fixture.Freeze<Mock<IValidator<SearchApprenticeshipVacanciesRequest>>>();
            _validatorMock
                .Setup(validator => validator.ValidateAsync(_validRequest, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult());

            _mockMapper = fixture.Freeze<Mock<IVacancySearchParametersMapper>>();
            _mockMapper
                .Setup(mapper => mapper.Convert(It.IsAny<SearchApprenticeshipVacanciesRequest>()))
                .Returns(_expectedParameters);

            _mockSearchService = fixture.Freeze<Mock<IApprenticeshipSearchService>>();
            _mockSearchService
                .Setup(service => service.SearchApprenticeshipVacanciesAsync(It.IsAny<VacancySearchParameters>()))
                .ReturnsAsync(_expectedResponse);

            _handler = fixture.Create<SearchApprenticeshipVacanciesQueryHandler>();

            _response = await _handler.Handle(_validRequest);
        }

        [Test]
        public void AndRequestNotValid_ThenThrowsValidationException()
        {
            var errorMessage = Guid.NewGuid().ToString();
            var invalidRequest = new SearchApprenticeshipVacanciesRequest();

            _validatorMock
                .Setup(validator => validator.ValidateAsync(invalidRequest, It.IsAny<CancellationToken>()))
                    .ReturnsAsync(new ValidationResult(new List<ValidationFailure>
                    {
                        new ValidationFailure("stuff", errorMessage)
                    }));

            var action = new Func<Task<SearchApprenticeshipVacanciesResponse>>(() =>
                _handler.Handle(invalidRequest));

            action.ShouldThrow<ValidationException>()
                .WithMessage($"Validation failed: \r\n -- {errorMessage}");
        }

        [Test]
        public void ThenValidatesRequest()
        {
            _validatorMock.Verify(validator => 
                validator.ValidateAsync(_validRequest, It.IsAny<CancellationToken>()), 
                Times.Once);
        }

        [Test]
        public void ThenMapsRequestToSearchParams()
        {
            _mockMapper.Verify(mapper => 
                mapper.Convert(_validRequest), 
                Times.Once);
        }

        [Test]
        public void ThenSearchesForVacancies()
        {
            _mockSearchService.Verify(service => 
                service.SearchApprenticeshipVacanciesAsync(_expectedParameters), 
                Times.Once);
        }

        [Test]
        public void ThenReturnsResultFromSearchService()
        {
            _response.Should().BeSameAs(_expectedResponse);
        }
    }
}