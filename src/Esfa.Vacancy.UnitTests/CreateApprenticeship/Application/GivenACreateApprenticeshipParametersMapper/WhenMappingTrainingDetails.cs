using Esfa.Vacancy.Application.Commands.CreateApprenticeship;
using Esfa.Vacancy.Domain.Entities;
using FluentAssertions;
using NUnit.Framework;
using NUnit.Framework.Internal;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.AutoMoq;

namespace Esfa.Vacancy.UnitTests.CreateApprenticeship.Application.GivenACreateApprenticeshipParametersMapper
{
    [TestFixture]
    public class WhenMappingTrainingDetails
    {
        [Test]
        public void AndTrainingTypeIsStandard_ThenMapTrainingCodeAsIs()
        {
            const string standardCode = "124";
            var fixture = new Fixture().Customize(new AutoMoqCustomization());
            var createApprenticeshipRequest = fixture.Build<CreateApprenticeshipRequest>()
                .With(request => request.TrainingType, TrainingType.Standard)
                .With(request => request.TrainingCode, standardCode)
                .Create();
            var employerInformation = fixture.Create<EmployerInformation>();

            var mapper = fixture.Create<CreateApprenticeshipParametersMapper>();

            var mappedParameters = mapper.MapFromRequest(createApprenticeshipRequest, employerInformation);

            mappedParameters.TrainingCode.Should().Be(standardCode);
        }

        [Test]
        public void AndTrainingTypeIsFramework_ThenMapFirstPartOfTrainingCode()
        {
            const string frameworkCode = "123-12-12";
            var fixture = new Fixture().Customize(new AutoMoqCustomization());
            var createApprenticeshipRequest = fixture.Build<CreateApprenticeshipRequest>()
                .With(request => request.TrainingType, TrainingType.Framework)
                .With(request => request.TrainingCode, frameworkCode)
                .Create();
            var employerInformation = fixture.Create<EmployerInformation>();

            var mapper = fixture.Create<CreateApprenticeshipParametersMapper>();

            var mappedParameters = mapper.MapFromRequest(createApprenticeshipRequest, employerInformation);

            mappedParameters.TrainingCode.Should().Be("123");
        }
    }
}