using Esfa.Vacancy.Application.Commands.CreateApprenticeship;
using Esfa.Vacancy.Domain.Entities;
using FluentAssertions;
using NUnit.Framework;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.AutoMoq;

namespace Esfa.Vacancy.UnitTests.CreateApprenticeship.Application.GivenACreateApprenticeshipParametersMapper
{
    public class WhenMappingApprenticeshipType
    {
        [TestCase(2, ApprenticeshipType.Intermediate)]
        [TestCase(3, ApprenticeshipType.Advanced)]
        [TestCase(4, ApprenticeshipType.Higher)]
        [TestCase(5, ApprenticeshipType.Foundation)]
        [TestCase(6, ApprenticeshipType.Degree)]
        [TestCase(7, ApprenticeshipType.Masters)]
        public void AndTrainingTypeIsStandard_ThenShouldBeNull(int apiLevel, ApprenticeshipType expectedLevel)
        {
            var fixture = new Fixture().Customize(new AutoMoqCustomization());
            var createApprenticeshipRequest = fixture.Build<CreateApprenticeshipRequest>()
                .With(request => request.TrainingType, TrainingType.Standard)
                .With(request => request.EducationLevel, apiLevel)
                .Create();
            var employerInformation = fixture.Create<EmployerInformation>();

            var mapper = fixture.Create<CreateApprenticeshipParametersMapper>();

            var mappedParameters = mapper.MapFromRequest(createApprenticeshipRequest, employerInformation);

            mappedParameters.ApprenticeshipType.Should().Be(expectedLevel);
        }

        [TestCase(2, ApprenticeshipType.Intermediate)]
        [TestCase(3, ApprenticeshipType.Advanced)]
        [TestCase(4, ApprenticeshipType.Higher)]
        [TestCase(5, ApprenticeshipType.Foundation)]
        [TestCase(6, ApprenticeshipType.Degree)]
        public void AndTrainingTypeIsFramework(int apiLevel, ApprenticeshipType expectedLevel)
        {
            var fixture = new Fixture().Customize(new AutoMoqCustomization());
            var createApprenticeshipRequest = fixture.Build<CreateApprenticeshipRequest>()
                .With(request => request.TrainingType, TrainingType.Framework)
                .With(request => request.EducationLevel, apiLevel)
                .Create();

            var employerInformation = fixture.Create<EmployerInformation>();

            var mapper = fixture.Create<CreateApprenticeshipParametersMapper>();

            var mappedParameters = mapper.MapFromRequest(createApprenticeshipRequest, employerInformation);

            mappedParameters.ApprenticeshipType.Should().Be(expectedLevel);
        }
    }
}