using System.Collections.Generic;
using System.Threading.Tasks;
using Esfa.Vacancy.Register.Application.Exceptions;
using Esfa.Vacancy.Register.Application.Interfaces;
using Esfa.Vacancy.Register.Application.Queries.GetVacancy;
using Esfa.Vacancy.Register.Domain.Entities;
using Esfa.Vacancy.Register.Domain.Repositories;
using FluentValidation;
using FluentValidation.Results;
using Moq;
using NUnit.Framework;
using SFA.DAS.NLog.Logger;


namespace Esfa.Vacancy.Register.UnitTests.Application.Queries.GetVacancy
{
    [TestFixture]
    public class WhenGettingVacancy
    {
        private Mock<ILog> _mockLogger;
        private Mock<IVacancyRepository> _mockVacancyRepository;
        private Mock<AbstractValidator<GetVacancyRequest>> _mockValidator;
        private Mock<ITrainingDetailService> _mockTrainingDetailService;
        private GetVacancyQueryHandler _queryHandler;

        [SetUp]
        public void Setup()
        {
            _mockLogger = new Mock<ILog>();
            _mockVacancyRepository = new Mock<IVacancyRepository>();
            _mockValidator = new Mock<AbstractValidator<GetVacancyRequest>>();
            _mockTrainingDetailService = new Mock<ITrainingDetailService>();
            _queryHandler = new GetVacancyQueryHandler(_mockValidator.Object,
                _mockVacancyRepository.Object, _mockLogger.Object, _mockTrainingDetailService.Object);
        }

        [Test]
        public void ThenIfInvalidRequest()
        {
            var failures = new List<ValidationFailure> { new ValidationFailure(string.Empty, string.Empty) };
            _mockValidator.Setup(v => v.Validate(It.IsAny<ValidationContext<GetVacancyRequest>>())).Returns(new ValidationResult(failures));

            Assert.ThrowsAsync<ValidationException>(async () => await _queryHandler.Handle(new GetVacancyRequest()));
        }

        [Test]
        public void ThenIfNoDataFound()
        {
            _mockValidator.Setup(v => v.Validate(It.IsAny<ValidationContext<GetVacancyRequest>>())).Returns(new ValidationResult());
            Domain.Entities.Vacancy vacancy = null;
            _mockVacancyRepository.Setup(r => r.GetVacancyByReferenceNumberAsync(It.IsAny<int>())).ReturnsAsync(vacancy);

            Assert.ThrowsAsync<ResourceNotFoundException>(async () => await _queryHandler.Handle(new GetVacancyRequest()));
        }

        [Test]
        public async Task ThenIfVacancyHasFrameworkIdGetFrameworkTitle()
        {
            _mockValidator
                .Setup(v => v.Validate(It.IsAny<ValidationContext<GetVacancyRequest>>()))
                .Returns(new ValidationResult());
            _mockTrainingDetailService
                .Setup(s => s.GetFrameworkDetailsAsync(It.IsAny<int>()))
                .ReturnsAsync(new Framework() { Title = "framework" });

            var vacancy = new Domain.Entities.Vacancy() { FrameworkCode = 123 };
            _mockVacancyRepository.Setup(r => r.GetVacancyByReferenceNumberAsync(It.IsAny<int>())).ReturnsAsync(vacancy);

            var response = await _queryHandler.Handle(new GetVacancyRequest());
            _mockTrainingDetailService.Verify(s => s.GetStandardDetailsAsync(It.IsAny<int>()), Times.Never);
            _mockTrainingDetailService.Verify(s => s.GetFrameworkDetailsAsync(It.IsAny<int>()));
            Assert.AreEqual(vacancy, response.Vacancy);
            Assert.IsNotNull(vacancy.Framework);
        }

        [Test]
        public async Task ThenIfVacancyHasStandardCodeGetStandardTitle()
        {
            _mockValidator
                .Setup(v => v.Validate(It.IsAny<ValidationContext<GetVacancyRequest>>()))
                .Returns(new ValidationResult());
            _mockTrainingDetailService
                .Setup(s => s.GetStandardDetailsAsync(It.IsAny<int>()))
                .ReturnsAsync(new Standard() { Title = "standard" });

            var vacancy = new Domain.Entities.Vacancy() { StandardCode = 123 };
            _mockVacancyRepository.Setup(r => r.GetVacancyByReferenceNumberAsync(It.IsAny<int>())).ReturnsAsync(vacancy);

            var response = await _queryHandler.Handle(new GetVacancyRequest());

            _mockTrainingDetailService.Verify(s => s.GetFrameworkDetailsAsync(It.IsAny<int>()), Times.Never);
            _mockTrainingDetailService.Verify(s => s.GetStandardDetailsAsync(It.IsAny<int>()));

            Assert.AreEqual(vacancy, response.Vacancy);
            Assert.IsNotNull(vacancy.Standard);
        }
    }
}
