using AutoMapper;
using Esfa.Vacancy.Api.Types;
using Esfa.Vacancy.Register.Api;
using Esfa.Vacancy.Register.Domain.Entities;
using NUnit.Framework;

namespace Esfa.Vacancy.Register.UnitTests.GetVacancy.Api.Mappings
{
    [TestFixture]
    public class VacancyMappingsTest
    {
        private IMapper _mapper;

        [OneTimeSetUp]
        public void Setup()
        {
            var config = AutoMapperConfig.Configure();
            _mapper = config.CreateMapper();
        }

        [Test]
        public void GivenFrameworkThenLoadFrameworkDetails()
        {
            var vacancy = new Domain.Entities.Vacancy
            {
                Standard = null,
                Framework = new Framework()
                {
                    Title = "Title",
                    Code = 13,
                    Uri = "sdfe"
                }
            };


            var result = _mapper.Map<Vacancy.Api.Types.Vacancy>(vacancy);

            Assert.AreEqual(TrainingType.Framework, result.TrainingType);
        }

        [Test]
        public void GivenStandardThenLoadStandardDetails()
        {
            var vacancy = new Domain.Entities.Vacancy
            {
                Framework = null,
                Standard = new Standard()
                {
                    Title = "Title",
                    Code = 13,
                    Uri = "sdfe"
                }
            };

            var result = _mapper.Map<Vacancy.Api.Types.Vacancy>(vacancy);

            Assert.AreEqual(TrainingType.Standard, result.TrainingType);
        }

        [Test]
        public void WhenFrameworkOrStandardIsMissingThenReturnUnavailable()
        {
            var vacancy = new Domain.Entities.Vacancy
            {
                Standard = null,
                Framework = null
            };

            var result = _mapper.Map<Vacancy.Api.Types.Vacancy>(vacancy);

            Assert.AreEqual(TrainingType.Unavailable, result.TrainingType);
        }
    }
}
