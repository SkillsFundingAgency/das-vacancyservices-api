using System;
using Esfa.Vacancy.Register.Infrastructure.Settings;
using Nest;

namespace Esfa.Vacancy.Register.Infrastructure.Factories
{
    public class ElasticClientFactory
    {
        private readonly IProvideSettings _provideSettings;

        public ElasticClientFactory(IProvideSettings provideSettings)
        {
            _provideSettings = provideSettings;
        }

        public IElasticClient GetClient()
        {
            var baseUri = _provideSettings.GetSetting(ApplicationSettingKeyConstants.VacancySearchUrlKey);
            var node = new Uri(baseUri);
            var settings = new ConnectionSettings(node);
            settings.SetTimeout(5000);

            return new ElasticClient(settings);
        }
    }
}