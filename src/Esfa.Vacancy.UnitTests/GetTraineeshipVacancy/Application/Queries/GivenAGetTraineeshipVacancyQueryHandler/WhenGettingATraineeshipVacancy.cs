using System.Collections.Generic;
using Esfa.Vacancy.Application.Exceptions;
using Esfa.Vacancy.Application.Queries.GetTraineeshipVacancy;
using Esfa.Vacancy.Domain.Interfaces;
using FluentValidation;
using FluentValidation.Results;
using Moq;
using NUnit.Framework;
using SFA.DAS.NLog.Logger;

namespace Esfa.Vacancy.UnitTests.GetTraineeshipVacancy.Application.Queries.GivenAGetTraineeshipVacancyQueryHandler
{
    [TestFixture]
    public class WhenGettingATraineeshipVacancy
    {
        private Mock<ILog> _mockLogger;
        private Mock<IGetTraineeshipService> _mockGetTraineeshipService;
        private Mock<AbstractValidator<GetTraineeshipVacancyRequest>> _mockValidator;
        private GetTraineeshipVacancyQueryHandler _queryHandler;

        [SetUp]
        public void Setup()
        {
            _mockLogger = new Mock<ILog>();
            _mockGetTraineeshipService = new Mock<IGetTraineeshipService>();
            _mockValidator = new Mock<AbstractValidator<GetTraineeshipVacancyRequest>>();
            _queryHandler = new GetTraineeshipVacancyQueryHandler(_mockValidator.Object,
                _mockGetTraineeshipService.Object, _mockLogger.Object);
        }

        [Test]
        public void ThenIfInvalidRequest()
        {
            var failures = new List<ValidationFailure> { new ValidationFailure(string.Empty, string.Empty) };
            _mockValidator.Setup(v => v.Validate(It.IsAny<ValidationContext<GetTraineeshipVacancyRequest>>())).Returns(new ValidationResult(failures));

            Assert.ThrowsAsync<ValidationException>(async () => await _queryHandler.Handle(new GetTraineeshipVacancyRequest()));
        }

        [Test]
        public void ThenIfNoDataFound()
        {
            _mockValidator.Setup(v => v.Validate(It.IsAny<ValidationContext<GetTraineeshipVacancyRequest>>())).Returns(new ValidationResult());
            Domain.Entities.TraineeshipVacancy traineeshipVacancy = null;
            _mockGetTraineeshipService.Setup(r => r.GetTraineeshipVacancyByReferenceNumberAsync(It.IsAny<int>())).ReturnsAsync(traineeshipVacancy);

            Assert.ThrowsAsync<ResourceNotFoundException>(async () => await _queryHandler.Handle(new GetTraineeshipVacancyRequest()));
        }
    }
}
