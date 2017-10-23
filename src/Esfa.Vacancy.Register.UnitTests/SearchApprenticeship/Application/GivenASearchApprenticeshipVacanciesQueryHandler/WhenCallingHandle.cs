using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Esfa.Vacancy.Register.Application.Interfaces;
using Esfa.Vacancy.Register.Application.Queries.SearchApprenticeshipVacancies;
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
        private SearchApprenticeshipVacanciesRequest _invalidRequest;
        private SearchApprenticeshipVacanciesRequest _validRequest;
        private string _errorMessage;
        private SearchApprenticeshipVacanciesResponse _expectedResponse;

        [SetUp]
        public void Setup()
        {
            _invalidRequest = new SearchApprenticeshipVacanciesRequest();
            _validRequest = new SearchApprenticeshipVacanciesRequest();
            _errorMessage = Guid.NewGuid().ToString();
            var searchParams = new VacancySearchParameters();
            _expectedResponse = new SearchApprenticeshipVacanciesResponse();

            var mockValidator = new Mock<IValidator<SearchApprenticeshipVacanciesRequest>>();
            mockValidator
                .Setup(validator => validator.ValidateAsync(_validRequest, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult());
            mockValidator
                .Setup(validator => validator.ValidateAsync(_invalidRequest, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult(new List<ValidationFailure>
                {
                    new ValidationFailure("stuff", _errorMessage)
                }));

            var mockConverter = new Mock<IVacancySearchParametersBuilder>();
            mockConverter
                .Setup(converter => converter.BuildAsync(_validRequest))
                .ReturnsAsync(searchParams);

            var mockSearchService = new Mock<IApprenticeshipSearchService>();
            mockSearchService
                .Setup(service => service.SearchApprenticeshipVacanciesAsync(searchParams))
                .ReturnsAsync(_expectedResponse);

            _handler = new SearchApprenticeshipVacanciesQueryHandler(
                mockValidator.Object,
                mockSearchService.Object,
                mockConverter.Object);
        }

        [Test]
        public void AndRequestNotValid_ThenThrowsValidationException()
        {
            var action = new Func<Task<SearchApprenticeshipVacanciesResponse>>(() =>
            _handler.Handle(_invalidRequest));

            action.ShouldThrow<ValidationException>()
                .WithMessage($"Validation failed: \r\n -- {_errorMessage}");
        }

        [Test]
        public void AndRequestValid_ThenReturnsResultFromSearchService()
        {
            _handler.Handle(_validRequest).Result
                .Should().BeSameAs(_expectedResponse);
        }
    }
}