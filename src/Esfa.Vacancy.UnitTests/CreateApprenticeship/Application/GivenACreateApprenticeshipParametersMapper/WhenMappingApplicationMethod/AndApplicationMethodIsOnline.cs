using Esfa.Vacancy.Application.Commands.CreateApprenticeship;
using Esfa.Vacancy.Domain.Entities;
using FluentAssertions;
using NUnit.Framework;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.AutoMoq;

namespace Esfa.Vacancy.UnitTests.CreateApprenticeship.Application.GivenACreateApprenticeshipParametersMapper.
    WhenMappingApplicationMethod
{
    [TestFixture]
    public class AndApplicationMethodIsOnline
    {
        private CreateApprenticeshipParameters _mappedParameters;
        private CreateApprenticeshipRequest _createApprenticeshipRequest;

        [SetUp]
        public void Setup()
        {
            var fixture = new Fixture().Customize(new AutoMoqCustomization());
            _createApprenticeshipRequest = fixture.Build<CreateApprenticeshipRequest>()
                                                  .With(request => request.ApplicationMethod, ApplicationMethod.Online)
                                                  .Create();
            var _employerInformation = fixture.Create<EmployerInformation>();

            var mapper = fixture.Create<CreateApprenticeshipParametersMapper>();
            _mappedParameters = mapper.MapFromRequest(_createApprenticeshipRequest, _employerInformation);
        }

        [Test]
        public void ThenMapApplicationMethodToOnline()
        {
            _mappedParameters.OfflineVacancyTypeId.Should().Be(1);
        }

        [Test]
        public void ThenMapSupplementaryQuestion1()
        {
            _mappedParameters.SupplementaryQuestion1.Should().Be(_createApprenticeshipRequest.SupplementaryQuestion1);
        }

        [Test]
        public void ThenMapSupplementaryQuestion2()
        {
            _mappedParameters.SupplementaryQuestion2.Should().Be(_createApprenticeshipRequest.SupplementaryQuestion2);
        }
    }
}
