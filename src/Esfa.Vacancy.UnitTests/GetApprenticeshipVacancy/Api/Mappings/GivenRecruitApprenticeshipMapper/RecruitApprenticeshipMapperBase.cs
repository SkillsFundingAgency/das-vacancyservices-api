using System;
using System.Globalization;
using Esfa.Vacancy.Application.Interfaces;
using Esfa.Vacancy.Domain.Constants;
using Esfa.Vacancy.Domain.Entities;
using Esfa.Vacancy.Domain.Interfaces;
using Esfa.Vacancy.Register.Api.Mappings;
using Moq;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.AutoMoq;
using SFA.DAS.Recruit.Vacancies.Client.Entities;

namespace Esfa.Vacancy.UnitTests.GetApprenticeshipVacancy.Api.Mappings.GivenRecruitApprenticeshipMapper
{
    public abstract class RecruitApprenticeshipMapperBase
    {
        protected Mock<IProvideSettings> ProvideSettingsMock;
        protected Mock<ITrainingDetailService> TrainingDetailServiceMock;
        protected Mock<IGetMinimumWagesService> MinimumWageServiceMock;
        protected IFixture FixtureInstance;
        protected LiveVacancy LiveVacancy;

        protected void Initialize()
        {
            FixtureInstance = new Fixture().Customize(new AutoMoqCustomization());

            ProvideSettingsMock = FixtureInstance.Freeze<Mock<IProvideSettings>>();            

            TrainingDetailServiceMock = FixtureInstance.Freeze<Mock<ITrainingDetailService>>();
            TrainingDetailServiceMock.Setup(t => t.GetStandardDetailsAsync(It.IsAny<int>()))
                .ReturnsAsync(FixtureInstance.Create<Standard>());

            MinimumWageServiceMock = FixtureInstance.Freeze<Mock<IGetMinimumWagesService>>();
            MinimumWageServiceMock.Setup(s => s.GetWageRangeAsync(It.IsAny<DateTime>()))
                .ReturnsAsync(FixtureInstance.Create<WageRange>());

            var wage = FixtureInstance.Build<Wage>()
                .With(w => w.WageType, RecruitApprenticeshipMapper.FixedWageType)
                .Create();
            LiveVacancy = FixtureInstance.Build<LiveVacancy>()
                .With(v => v.Wage, wage)
                .With(v => v.ProgrammeType, "Standard")
                .With(v => v.ProgrammeId, "123")
                .Create();
        }

        protected internal RecruitApprenticeshipMapper GetRecruitApprecticeshipMapper()
        {
            return FixtureInstance.Create<RecruitApprenticeshipMapper>();
        }

        protected string GetFormattedCurrencyString(decimal src)
        {
            const string currencyStringFormat = "C";
            return src.ToString(currencyStringFormat, CultureInfo.GetCultureInfo("en-GB"));
        }
    }
}