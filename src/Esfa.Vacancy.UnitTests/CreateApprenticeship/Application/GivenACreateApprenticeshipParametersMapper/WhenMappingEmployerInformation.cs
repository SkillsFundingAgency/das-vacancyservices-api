using Esfa.Vacancy.Application.Commands.CreateApprenticeship;
using Esfa.Vacancy.Domain.Entities;
using FluentAssertions;
using NUnit.Framework;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.AutoMoq;

namespace Esfa.Vacancy.UnitTests.CreateApprenticeship.Application.GivenACreateApprenticeshipParametersMapper
{
    [TestFixture]
    public class WhenMappingEmployerInformation
    {
        private IFixture _fixture;

        [SetUp]
        public void InitializeTest()
        {
            _fixture = new Fixture().Customize(new AutoMoqCustomization());
        }

        [Test]
        public void AndEmployerDescriptionExistsInTheRequest()
        {
            var mapper = _fixture.Create<CreateApprenticeshipParametersMapper>();
            var request = _fixture
                .Build<CreateApprenticeshipRequest>()
                .Create();
            var empInfo = _fixture
                .Build<EmployerInformation>()
                .Create();
            var parameters = mapper.MapFromRequest(request, empInfo);

            parameters.EmployerDescription.Should().Be(request.EmployerDescription);
        }

        [Test]
        public void AndEmployerDescriptionIsNotInTheRequest()
        {
            var mapper = _fixture.Create<CreateApprenticeshipParametersMapper>();
            var request = _fixture
                .Build<CreateApprenticeshipRequest>()
                .Without(r => r.EmployerDescription)
                .Create();
            var empInfo = _fixture
                .Build<EmployerInformation>()
                .Create();
            var parameters = mapper.MapFromRequest(request, empInfo);

            parameters.EmployerDescription.Should().Be(empInfo.EmployerDescription);
        }

        [Test]
        public void AndEmployerWebsiteExistsInTheRequest()
        {
            var mapper = _fixture.Create<CreateApprenticeshipParametersMapper>();
            var request = _fixture
                .Build<CreateApprenticeshipRequest>()
                .Create();
            var empInfo = _fixture
                .Build<EmployerInformation>()
                .Create();
            var parameters = mapper.MapFromRequest(request, empInfo);

            parameters.EmployerWebsite.Should().Be(request.EmployersWebsiteUrl);
        }

        [Test]
        public void AndEmployerWebsiteIsNotInTheRequest()
        {
            var mapper = _fixture.Create<CreateApprenticeshipParametersMapper>();
            var request = _fixture
                .Build<CreateApprenticeshipRequest>()
                .Without(r => r.EmployersWebsiteUrl)
                .Create();
            var empInfo = _fixture
                .Build<EmployerInformation>()
                .Create();
            var parameters = mapper.MapFromRequest(request, empInfo);

            parameters.EmployerWebsite.Should().Be(empInfo.EmployerWebsite);
        }
    }
}