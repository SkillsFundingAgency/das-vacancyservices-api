using System.Collections.Generic;
using System.Threading.Tasks;
using Esfa.Vacancy.Application.Exceptions;
using Esfa.Vacancy.Application.Interfaces;
using Esfa.Vacancy.Application.Queries.GetApprenticeshipVacancy;
using Esfa.Vacancy.Domain.Entities;
using Esfa.Vacancy.Domain.Interfaces;
using FluentValidation;
using FluentValidation.Results;
using Moq;
using NUnit.Framework;
using SFA.DAS.NLog.Logger;

namespace Esfa.Vacancy.UnitTests.GetApprenticeshipVacancy.Application.Queries.GivenAGetApprenticeshipVacancyQueryHandler
{
    [TestFixture]
    public class WhenGettingApprenticeshipVacancy
    {
        private Mock<ILog> _mockLogger;
        private Mock<IGetApprenticeshipService> _mockGetApprenticeshipService;
        private Mock<AbstractValidator<GetApprenticeshipVacancyRequest>> _mockValidator;
        private Mock<ITrainingDetailService> _mockTrainingDetailService;
        private GetApprenticeshipVacancyQueryHandler _queryHandler;

        [SetUp]
        public void Setup()
        {
            _mockLogger = new Mock<ILog>();
            _mockGetApprenticeshipService = new Mock<IGetApprenticeshipService>();
            _mockValidator = new Mock<AbstractValidator<GetApprenticeshipVacancyRequest>>();
            _mockTrainingDetailService = new Mock<ITrainingDetailService>();
            _queryHandler = new GetApprenticeshipVacancyQueryHandler(_mockValidator.Object,
                _mockGetApprenticeshipService.Object, _mockLogger.Object, _mockTrainingDetailService.Object);
        }

        [Test]
        public void ThenIfInvalidRequest()
        {
            var failures = new List<ValidationFailure> { new ValidationFailure(string.Empty, string.Empty) };
            _mockValidator.Setup(v => v.Validate(It.IsAny<ValidationContext<GetApprenticeshipVacancyRequest>>())).Returns(new ValidationResult(failures));

            Assert.ThrowsAsync<ValidationException>(async () => await _queryHandler.Handle(new GetApprenticeshipVacancyRequest()));
        }

        [Test]
        public void ThenIfNoDataFound()
        {
            _mockValidator.Setup(v => v.Validate(It.IsAny<ValidationContext<GetApprenticeshipVacancyRequest>>())).Returns(new ValidationResult());
            Domain.Entities.ApprenticeshipVacancy apprenticeshipVacancy = null;
            _mockGetApprenticeshipService.Setup(r => r.GetApprenticeshipVacancyByReferenceNumberAsync(It.IsAny<int>())).ReturnsAsync(apprenticeshipVacancy);

            Assert.ThrowsAsync<ResourceNotFoundException>(async () => await _queryHandler.Handle(new GetApprenticeshipVacancyRequest()));
        }

        [Test]
        public async Task ThenIfVacancyHasFrameworkIdGetFrameworkTitle()
        {
            _mockValidator
                .Setup(v => v.Validate(It.IsAny<ValidationContext<GetApprenticeshipVacancyRequest>>()))
                .Returns(new ValidationResult());
            _mockTrainingDetailService
                .Setup(s => s.GetFrameworkDetailsAsync(It.IsAny<int>()))
                .ReturnsAsync(new Framework() { Title = "framework" });

            var vacancy = new Domain.Entities.ApprenticeshipVacancy() { FrameworkCode = 123 };
            _mockGetApprenticeshipService.Setup(r => r.GetApprenticeshipVacancyByReferenceNumberAsync(It.IsAny<int>())).ReturnsAsync(vacancy);

            var response = await _queryHandler.Handle(new GetApprenticeshipVacancyRequest());
            _mockTrainingDetailService.Verify(s => s.GetStandardDetailsAsync(It.IsAny<int>()), Times.Never);
            _mockTrainingDetailService.Verify(s => s.GetFrameworkDetailsAsync(It.IsAny<int>()));
            Assert.AreEqual(vacancy, response.ApprenticeshipVacancy);
            Assert.IsNotNull(vacancy.Framework);
        }

        [Test]
        public async Task ThenIfVacancyHasStandardCodeGetStandardTitle()
        {
            _mockValidator
                .Setup(v => v.Validate(It.IsAny<ValidationContext<GetApprenticeshipVacancyRequest>>()))
                .Returns(new ValidationResult());
            _mockTrainingDetailService
                .Setup(s => s.GetStandardDetailsAsync(It.IsAny<int>()))
                .ReturnsAsync(new Standard() { Title = "standard" });

            var vacancy = new Domain.Entities.ApprenticeshipVacancy() { StandardCode = 123 };
            _mockGetApprenticeshipService.Setup(r => r.GetApprenticeshipVacancyByReferenceNumberAsync(It.IsAny<int>())).ReturnsAsync(vacancy);

            var response = await _queryHandler.Handle(new GetApprenticeshipVacancyRequest());

            _mockTrainingDetailService.Verify(s => s.GetFrameworkDetailsAsync(It.IsAny<int>()), Times.Never);
            _mockTrainingDetailService.Verify(s => s.GetStandardDetailsAsync(It.IsAny<int>()));

            Assert.AreEqual(vacancy, response.ApprenticeshipVacancy);
            Assert.IsNotNull(vacancy.Standard);
        }
    }
}
