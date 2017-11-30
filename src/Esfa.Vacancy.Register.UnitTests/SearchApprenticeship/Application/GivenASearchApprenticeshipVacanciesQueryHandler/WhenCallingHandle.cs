using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Esfa.Vacancy.Application.Interfaces;
using Esfa.Vacancy.Application.Queries.SearchApprenticeshipVacancies;
using Esfa.Vacancy.Register.Domain.Entities;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Moq;
using NUnit.Framework;

namespace Esfa.Vacancy.Register.UnitTests.SearchApprenticeship.Application.GivenASearchApprenticeshipVacanciesQueryHandler
{
    [TestFixture]
    public class WhenCallingHandle
    {
        private SearchApprenticeshipVacanciesQueryHandler _handler;
        private SearchApprenticeshipVacanciesResponse _expectedResponse;
        private Mock<IValidator<SearchApprenticeshipVacanciesRequest>> _validatorMock;

        [SetUp]
        public void Setup()
        {
            _expectedResponse = new SearchApprenticeshipVacanciesResponse();

            _validatorMock = new Mock<IValidator<SearchApprenticeshipVacanciesRequest>>();

            var mockSearchService = new Mock<IApprenticeshipSearchService>();
            mockSearchService
                .Setup(service => service.SearchApprenticeshipVacanciesAsync(It.IsAny<VacancySearchParameters>()))
                .ReturnsAsync(_expectedResponse);

            _handler = new SearchApprenticeshipVacanciesQueryHandler(
                _validatorMock.Object,
                mockSearchService.Object);
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
        public async Task AndRequestValid_ThenReturnsResultFromSearchService()
        {
            var validRequest = new SearchApprenticeshipVacanciesRequest();

            _validatorMock
                .Setup(validator => validator.ValidateAsync(validRequest, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult());

            var response = await _handler.Handle(validRequest);

            response.Should().BeSameAs(_expectedResponse);
        }
    }
}