using Esfa.Vacancy.Api.Types;
using Esfa.Vacancy.Register.Api.Mappings;
using Esfa.Vacancy.Register.Domain.Entities;
using Esfa.Vacancy.Register.Infrastructure.Settings;
using Moq;
using NUnit.Framework;
using Ploeh.AutoFixture;

namespace Esfa.Vacancy.Register.UnitTests.GetApprenticeshipVacancy.Api.Mappings.GivenAnApprenticeshipMapper
{
    [TestFixture]
    public class WhenGettingLiveVacancyTrainingDetails
    {
        [Test]
        public void GivenFrameworkThenLoadFrameworkDetails()
        {
            var vacancy = new Fixture().Build<Domain.Entities.ApprenticeshipVacancy>()
                                        .Without(v => v.Standard)
                                        .With(v => v.Framework, new Framework
                                        {
                                            Title = "Title",
                                            Code = 13,
                                            Uri = "sdfe"
                                        })
                                        .Create();

            var sut = new ApprenticeshipMapper(Mock.Of<IProvideSettings>());

            var result = sut.MapToApprenticeshipVacancy(vacancy);

            Assert.AreEqual(TrainingType.Framework, result.TrainingType);
        }

        [Test]
        public void GivenStandardThenLoadStandardDetails()
        {
            var vacancy = new Fixture().Build<Domain.Entities.ApprenticeshipVacancy>()
                                        .Without(v => v.Framework)
                                        .With(v => v.Standard, new Standard
                                        {
                                            Title = "Title",
                                            Code = 13,
                                            Uri = "sdfe"

                                        })
                                        .Create();

            var sut = new ApprenticeshipMapper(Mock.Of<IProvideSettings>());

            var result = sut.MapToApprenticeshipVacancy(vacancy);

            Assert.AreEqual(TrainingType.Standard, result.TrainingType);
        }

        [Test]
        public void WhenFrameworkOrStandardIsMissingThenReturnUnavailable()
        {
            var vacancy = new Fixture().Build<Domain.Entities.ApprenticeshipVacancy>()
                                        .Without(v => v.Framework)
                                        .Without(v => v.Standard)
                                        .Create();

            var sut = new ApprenticeshipMapper(Mock.Of<IProvideSettings>());

            var result = sut.MapToApprenticeshipVacancy(vacancy);

            Assert.AreEqual(TrainingType.Unavailable, result.TrainingType);
        }
    }
}
