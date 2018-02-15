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
    public class WhenApplicationMethodIsOffline
    {
        private CreateApprenticeshipParameters _mappedParameters;
        private CreateApprenticeshipRequest _createApprenticeshipRequest;

        [SetUp]
        public void Setup()
        {
            var fixture = new Fixture().Customize(new AutoMoqCustomization());
            _createApprenticeshipRequest = fixture.Build<CreateApprenticeshipRequest>()
                                                  .With(request => request.ApplicationMethod, ApplicationMethod.Offline)
                                                  .Create();
            var _employerInformation = fixture.Create<EmployerInformation>();

            var mapper = fixture.Create<CreateApprenticeshipParametersMapper>();
            _mappedParameters = mapper.MapFromRequest(_createApprenticeshipRequest, _employerInformation);
        }

        [Test]
        public void ThenMapApplicationMethodToOffline()
        {
            _mappedParameters.ApplyOutsideNAVMS.Should().BeTrue();
        }

        [Test]
        public void ThenMapExternalApplicationUrl()
        {
            _mappedParameters.EmployersRecruitmentWebsite.Should().Be(_createApprenticeshipRequest.ExternalApplicationUrl);
        }

        [Test]
        public void ThenMapExternalApplicationInstructions()
        {
            _mappedParameters.EmployersApplicationInstructions.Should()
                             .Be(_createApprenticeshipRequest.ExternalApplicationInstructions);
        }
    }
}
