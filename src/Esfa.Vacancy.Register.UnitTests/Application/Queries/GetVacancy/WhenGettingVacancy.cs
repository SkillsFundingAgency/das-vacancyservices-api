using System.Collections.Generic;
using System.Threading.Tasks;
using Esfa.Vacancy.Register.Application.Exceptions;
using Esfa.Vacancy.Register.Application.Queries.GetVacancy;
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
        private readonly Mock<ILog> _logger = new Mock<ILog>();
        private readonly Mock<IVacancyRepository> _vacancyRepository = new Mock<IVacancyRepository>();
        private readonly Mock<AbstractValidator<GetVacancyRequest>> _validator = new Mock<AbstractValidator<GetVacancyRequest>>();
        private GetVacancyQueryHandler _queryHandler;

        [SetUp]
        public void Setup()
        {
            _queryHandler = new GetVacancyQueryHandler(_validator.Object, _vacancyRepository.Object, _logger.Object);
        }

        [Test]
        public async Task ThenIfInvalidRequest()
        {
            var failures = new List<ValidationFailure> { new ValidationFailure(string.Empty, string.Empty) };
            _validator.Setup(v => v.Validate(It.IsAny<ValidationContext<GetVacancyRequest>>())).Returns(new ValidationResult(failures));

            Assert.ThrowsAsync<ValidationException>(async () => await _queryHandler.Handle(new GetVacancyRequest()));
        }

        [Test]
        public async Task ThenIfNoDataFound()
        {
            _validator.Setup(v => v.Validate(It.IsAny<ValidationContext<GetVacancyRequest>>())).Returns(new ValidationResult());
            Domain.Entities.Vacancy vacancy = null;
            _vacancyRepository.Setup(r => r.GetVacancyByReferenceNumberAsync(It.IsAny<int>())).ReturnsAsync(vacancy);

            Assert.ThrowsAsync<ResourceNotFoundException>(async () => await _queryHandler.Handle(new GetVacancyRequest()));
        }

        [Test]
        public async Task ThenIfDataFoundReturnVacancy()
        {
            _validator.Setup(v => v.Validate(It.IsAny<ValidationContext<GetVacancyRequest>>())).Returns(new ValidationResult());
            var vacancy = new Domain.Entities.Vacancy();
            _vacancyRepository.Setup(r => r.GetVacancyByReferenceNumberAsync(It.IsAny<int>())).ReturnsAsync(vacancy);

            var response = await _queryHandler.Handle(new GetVacancyRequest());
            Assert.AreEqual(vacancy, response.Vacancy);
        }
    }
}
