using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Esfa.Vacancy.Register.Application.Exceptions;
using Esfa.Vacancy.Register.Application.Queries.GetVacancy;
using Esfa.Vacancy.Register.Domain.Entities;
using Esfa.Vacancy.Register.Domain.Interfaces;
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
        private int _refNumberForFramework;
        private int _refNumberForStandard;
        private Domain.Entities.Vacancy _vacancyWithFrameworkCode;
        private Domain.Entities.Vacancy _vacancyWithStandardCode;


        [SetUp]
        public void Setup()
        {
            _refNumberForFramework = 98098;
            _refNumberForStandard = 932478;
            _vacancyWithFrameworkCode = new Domain.Entities.Vacancy() { FrameworkCode = 435645 };
            _vacancyWithStandardCode = new Domain.Entities.Vacancy() { StandardCode = 435645 };

            _mockLogger = new Mock<ILog>();

            _mockVacancyRepository = new Mock<IVacancyRepository>();
            _mockVacancyRepository
                .Setup(r => r.GetVacancyByReferenceNumberAsync(_refNumberForFramework))
                .ReturnsAsync(_vacancyWithFrameworkCode);
            _mockVacancyRepository
                .Setup(r => r.GetVacancyByReferenceNumberAsync(_refNumberForStandard))
                .ReturnsAsync(_vacancyWithStandardCode);

            _mockValidator = new Mock<AbstractValidator<GetVacancyRequest>>();
            _mockValidator
                .Setup(v => v.Validate(It.IsAny<ValidationContext<GetVacancyRequest>>()))
                .Returns(new ValidationResult());

            _mockTrainingDetailService = new Mock<ITrainingDetailService>();
            _mockTrainingDetailService
                .Setup(s => s.GetFrameworkDetailsAsync(_vacancyWithFrameworkCode.FrameworkCode.Value))
                .ReturnsAsync(new Framework() { Title = "framework" });
            _mockTrainingDetailService
                .Setup(s => s.GetStandardDetailsAsync(_vacancyWithStandardCode.StandardCode.Value))
                .ReturnsAsync(new Standard() { Title = "standard" });

            _queryHandler = new GetVacancyQueryHandler(_mockValidator.Object,
                _mockVacancyRepository.Object, _mockLogger.Object, _mockTrainingDetailService.Object);
        }

        [Test]
        public void ThenIfInvalidRequest()
        {
            var failures = new List<ValidationFailure> { new ValidationFailure(string.Empty, string.Empty) };
            _mockValidator
                .Setup(v => v.Validate(It.IsAny<ValidationContext<GetVacancyRequest>>()))
                .Returns(new ValidationResult(failures));

            Assert.ThrowsAsync<ValidationException>(async () => await _queryHandler.Handle(new GetVacancyRequest()));
        }

        [Test]
        public void ThenIfNoDataFound()
        {
            _mockVacancyRepository
                .Setup(r => r.GetVacancyByReferenceNumberAsync(It.IsAny<int>()))
                .ReturnsAsync((Domain.Entities.Vacancy)null);

            Assert.ThrowsAsync<ResourceNotFoundException>(async () => await _queryHandler.Handle(new GetVacancyRequest()));
        }

        [Test]
        public async Task ThenIfVacancyHasFrameworkIdGetFrameworkTitle()
        {
            var response = await _queryHandler.Handle(new GetVacancyRequest{Reference = _refNumberForFramework});

            _mockTrainingDetailService.Verify(s => s.GetStandardDetailsAsync(It.IsAny<int>()), Times.Never);
            _mockTrainingDetailService.Verify(s => s.GetFrameworkDetailsAsync(It.IsAny<int>()));

            Assert.AreEqual(_vacancyWithFrameworkCode, response.Vacancy);
            Assert.IsNotNull(_vacancyWithFrameworkCode.Framework);
        }

        [Test]
        public async Task ThenIfVacancyHasStandardCodeGetStandardTitle()
        {
            var response = await _queryHandler.Handle(new GetVacancyRequest{Reference = _refNumberForStandard});

            _mockTrainingDetailService.Verify(s => s.GetFrameworkDetailsAsync(It.IsAny<int>()), Times.Never);
            _mockTrainingDetailService.Verify(s => s.GetStandardDetailsAsync(It.IsAny<int>()));

            Assert.AreEqual(_vacancyWithStandardCode, response.Vacancy);
            Assert.IsNotNull(_vacancyWithStandardCode.Standard);
        }
    }
}
