using Esfa.Vacancy.Domain.Constants;
using Esfa.Vacancy.Domain.Interfaces;
using Esfa.Vacancy.Infrastructure.Settings;
using SFA.DAS.Elastic;
using SFA.DAS.VacancyServices.Search;
using System;

namespace Esfa.Vacancy.Infrastructure.Ioc
{
    public sealed class ElasticClientRegistry : StructureMap.Registry
    {
        public ElasticClientRegistry()
            : this(new AppConfigSettingsProvider(new MachineSettings()))
        {
        }

        public ElasticClientRegistry(IProvideSettings _provideSettings)
        {
            var username = _provideSettings.GetNullableSetting(ApplicationSettingKeys.ElasticUsernameKey);
            var password = _provideSettings.GetNullableSetting(ApplicationSettingKeys.ElasticPasswordKey);
            var indexName = _provideSettings.GetSetting(ApplicationSettingKeys.ApprenticeshipIndexAliasKey);

            ElasticClientConfiguration elasticConfig;
#if DEBUG
            var hostUrl = _provideSettings.GetSetting(ApplicationSettingKeys.VacancySearchUrlKey);

            elasticConfig = string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password)
                ? new ElasticClientConfiguration(new Uri(hostUrl))
                : new ElasticClientConfiguration(new Uri(hostUrl), username, password);
#else
            var cloudId = _provideSettings.GetSetting(ApplicationSettingKeys.ElasticCloudIdKey);
            elasticConfig = new ElasticClientConfiguration(cloudId, username, password);
#endif
            For<ElasticClientConfiguration>().Use(elasticConfig);
            For<IElasticClientFactory>().Use<ElasticClientFactory>();
            For<IApprenticeshipSearchClient>().Use<ApprenticeshipSearchClient>().Ctor<string>("indexName").Is(indexName);
        }
    }
}
