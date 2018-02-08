using System;
using Esfa.Vacancy.Domain.Constants;
using Esfa.Vacancy.Domain.Interfaces;
using Nest;

namespace Esfa.Vacancy.Infrastructure.Factories
{
    public class ElasticClientFactory
    {
        private readonly IProvideSettings _provideSettings;
        private const int ElasticClientTimeoutMilliseconds = 5000;

        public ElasticClientFactory(IProvideSettings provideSettings)
        {
            _provideSettings = provideSettings;
        }

        public IElasticClient GetClient()
        {
            var baseUri = _provideSettings.GetSetting(ApplicationSettingKeys.VacancySearchUrlKey);
            var node = new Uri(baseUri);
            var settings = new ConnectionSettings(node);

            settings.SetTimeout(ElasticClientTimeoutMilliseconds);

            return new ElasticClient(settings);
        }
    }
}