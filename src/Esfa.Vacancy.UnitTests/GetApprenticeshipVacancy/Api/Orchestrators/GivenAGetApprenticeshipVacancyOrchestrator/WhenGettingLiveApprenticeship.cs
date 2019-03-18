using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Esfa.Vacancy.Api.Core.Validation;
using Esfa.Vacancy.Application.Queries.GetApprenticeshipVacancy;
using Esfa.Vacancy.Domain.Entities;
using Esfa.Vacancy.Domain.Validation;
using Esfa.Vacancy.Register.Api.Mappings;
using Esfa.Vacancy.Register.Api.Orchestrators;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Moq;
using NUnit.Framework;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.AutoMoq;
using SFA.DAS.Recruit.Vacancies.Client;

namespace Esfa.Vacancy.UnitTests.GetApprenticeshipVacancy.Api.Orchestrators.GivenAGetApprenticeshipVacancyOrchestrator
{
    [TestFixture]
    [Parallelizable(ParallelScope.Fixtures)]
    public class WhenGettingLiveApprenticeship
    {
        private Mock<IMediator> _mockMediator;
        private GetApprenticeshipVacancyOrchestrator _sut;
        private IFixture _fixture;
        private string _expectedErrorMessage;
        private Mock<IValidationExceptionBuilder> _mockValidationExceptionBuilder;
        private Mock<IClient> _mockClient;
        private Mock<IApprenticeshipMapper> _mockMapper;
        private Mock<IRecruitVacancyMapper> _recuitMapperMock;

        [SetUp]
        public void SetUp()
        {
            _fixture = new Fixture().Customize(new AutoMoqCustomization());

            _expectedErrorMessage = _fixture.Create<string>();

            _mockMediator = _fixture.Freeze<Mock<IMediator>>(composer => composer.Do(mock => mock
                .Setup(mediator => mediator.Send(It.IsAny<GetApprenticeshipVacancyRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(_fixture.Create<GetApprenticeshipVacancyResponse>())));

            _mockValidationExceptionBuilder = _fixture.Freeze<Mock<IValidationExceptionBuilder>>(composer => composer.Do(mock => mock
                .Setup(builder => builder.Build(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(new ValidationException(new List<ValidationFailure>
                {
                    new ValidationFailure("", _expectedErrorMessage)
                }))
            ));

            _mockClient = _fixture.Freeze<Mock<IClient>>();
            _mockMapper = _fixture.Freeze<Mock<IApprenticeshipMapper>>();
            _recuitMapperMock = _fixture.Freeze<Mock<IRecruitVacancyMapper>>();

            _sut = _fixture.Create<GetApprenticeshipVacancyOrchestrator>();
        }

        [Test]
        public async Task ThenCreatesGetApprenticeshipVacancyRequestWithRefNumber()
        {
            var uniqueVacancyRef = _fixture.Create<int>();
            await _sut.GetApprenticeshipVacancyDetailsAsync(uniqueVacancyRef.ToString());

            _mockMediator.Verify(mediator =>
                mediator.Send(
                    It.Is<GetApprenticeshipVacancyRequest>(request => request.Reference == uniqueVacancyRef),
                    CancellationToken.None));

            _mockMapper.Verify(m => m.MapToApprenticeshipVacancy(It.IsAny<ApprenticeshipVacancy>()));

            _mockClient.Verify(m => m.GetVacancy(It.IsAny<long>()), Times.Never);
            _recuitMapperMock.Verify(m => m.MapFromRecruitVacancy(It.IsAny<SFA.DAS.Recruit.Vacancies.Client.Entities.Vacancy>()), Times.Never);
        }

        [Test]
        public void AndParamIsNotAnInt32_ThenThrowsValidationException()
        {
            Func<Task> action = async () =>
            {
                await _sut.GetApprenticeshipVacancyDetailsAsync(Guid.NewGuid().ToString());
            };

            action.ShouldThrow<ValidationException>()
                .WithMessage($"Validation failed: \r\n -- {_expectedErrorMessage}");

            _mockValidationExceptionBuilder.Verify(builder =>
                builder.Build(
                    ErrorCodes.GetApprenticeship.VacancyReferenceNumberNotInt32,
                    ErrorMessages.GetApprenticeship.VacancyReferenceNumberNotNumeric,
                    It.IsAny<string>()), Times.Once);
        }
    }
}
