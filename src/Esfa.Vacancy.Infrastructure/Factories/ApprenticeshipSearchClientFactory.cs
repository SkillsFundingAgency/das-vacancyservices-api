using Esfa.Vacancy.Domain.Constants;
using Esfa.Vacancy.Domain.Interfaces;
using SFA.DAS.VacancyServices.Search;

namespace Esfa.Vacancy.Infrastructure.Factories
{
    public class ApprenticeshipSearchClientFactory
    {
        private readonly IProvideSettings _provideSettings;

        public ApprenticeshipSearchClientFactory(IProvideSettings provideSettings)
        {
            _provideSettings = provideSettings;
        }

        public IApprenticeshipSearchClient GetClient()
        {
            var config = new ApprenticeshipSearchClientConfiguration
            {
                HostName = _provideSettings.GetSetting(ApplicationSettingKeys.VacancySearchUrlKey),
                Index = _provideSettings.GetSetting(ApplicationSettingKeys.ApprenticeshipIndexAliasKey),
            };

            return new ApprenticeshipSearchClient(config);
        }
    }
}
