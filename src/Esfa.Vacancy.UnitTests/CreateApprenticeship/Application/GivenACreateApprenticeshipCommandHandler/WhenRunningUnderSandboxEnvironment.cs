using System.Threading;
using System.Threading.Tasks;
using Esfa.Vacancy.Application.Commands.CreateApprenticeship;
using Esfa.Vacancy.Domain.Constants;
using Esfa.Vacancy.Domain.Entities;
using Esfa.Vacancy.Domain.Interfaces;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Moq;
using NUnit.Framework;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.AutoMoq;

namespace Esfa.Vacancy.UnitTests.CreateApprenticeship.Application.GivenACreateApprenticeshipCommandHandler
{
    [TestFixture]
    public class WhenRunningUnderSandboxEnvironment
    {
        private EmployerInformation _employerInformation;
        private CreateApprenticeshipResponse _createApprenticeshipResponse;
        private int _defaultReferenceNumber = 0;
        private Mock<IValidator<CreateApprenticeshipRequest>> _mockValidator;
        private IFixture _fixture;
        private CreateApprenticeshipCommandHandler _handler;
        private Mock<ICreateApprenticeshipParametersMapper> _mockMapper;
        private Mock<ICreateApprenticeshipService> _mockService;
        private CreateApprenticeshipRequest _validRequest;
        private CreateApprenticeshipParameters _expectedParameters;
        private Mock<IVacancyOwnerService> _mockVacancyOwnerService;
        private Mock<IProvideSettings> _mockProvideSettings;

        [SetUp]
        public async Task SetUp()
        {
            _fixture = new Fixture().Customize(new AutoMoqCustomization());

            _employerInformation = _fixture.Create<EmployerInformation>();
            _expectedParameters = _fixture.Freeze<CreateApprenticeshipParameters>();
            _validRequest = _fixture.Create<CreateApprenticeshipRequest>();

            _mockValidator = _fixture.Freeze<Mock<IValidator<CreateApprenticeshipRequest>>>(composer => composer.Do(mock => mock
                .Setup(validator => validator.ValidateAsync(_validRequest, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult())));

            _mockVacancyOwnerService = _fixture.Freeze<Mock<IVacancyOwnerService>>(composer => composer.Do(mock => mock
                .Setup(svc => svc.GetEmployersInformationAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(_employerInformation)));

            _mockMapper = _fixture.Freeze<Mock<ICreateApprenticeshipParametersMapper>>(composer => composer.Do(mock => mock
                .Setup(mapper => mapper.MapFromRequest(It.IsAny<CreateApprenticeshipRequest>(), It.IsAny<EmployerInformation>()))
                .Returns(_expectedParameters)));

            _mockService = _fixture.Freeze<Mock<ICreateApprenticeshipService>>();

            _mockProvideSettings = _fixture.Freeze<Mock<IProvideSettings>>(composer => composer.Do(mock => mock
                .Setup(appSettings => appSettings.GetNullableSetting(ApplicationSettingKeys.IsSandboxEnvironment))
                .Returns("can be any value")));

            _handler = _fixture.Create<CreateApprenticeshipCommandHandler>();

            _createApprenticeshipResponse = await _handler.Handle(_validRequest);
        }

        [Test]
        public void ThenValidatesRequest()
        {
            _mockValidator.Verify(validator =>
                    validator.ValidateAsync(_validRequest, It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Test]
        public void ThenFetchVacancyOwnerLink()
        {
            _mockVacancyOwnerService.Verify(svc => svc.GetEmployersInformationAsync(
                It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()));
        }

        [Test]
        public void ThenMapsRequestToCreateParams()
        {
            _mockMapper.Verify(mapper =>
                    mapper.MapFromRequest(_validRequest, _employerInformation),
                Times.Once);
        }

        [Test]
        public void ThenCallsProvideSettings()
        {
            _mockProvideSettings.Verify(svc =>
                svc.GetNullableSetting(ApplicationSettingKeys.IsSandboxEnvironment));
        }

        [Test]
        public void ThenAvoidsCreateVacancy()
        {
            _mockService.Verify(repository =>
                    repository.CreateApprenticeshipAsync(_expectedParameters),
                Times.Never);
        }

        [Test]
        public void ThenReturnsDefaultReferenceNumber()
        {
            _createApprenticeshipResponse.VacancyReferenceNumber
                .Should().Be(_defaultReferenceNumber);
        }
    }
}