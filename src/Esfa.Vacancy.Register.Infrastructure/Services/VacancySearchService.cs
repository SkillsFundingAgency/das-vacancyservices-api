using System;
using Esfa.Vacancy.Register.Infrastructure.Settings;
using Nest;
using SFA.DAS.NLog.Logger;

namespace Esfa.Vacancy.Register.Infrastructure.Services
{
    public class VacancySearchService
    {
        private readonly IProvideSettings _provideSettings;
        private readonly ILog _logger;

        public VacancySearchService(IProvideSettings provideSettings, ILog logger)
        {
            _provideSettings = provideSettings;
            _logger = logger;
        }


        private ElasticClient GetElasticSearchClient(string indexName)
        {
            var baseUri = _provideSettings.GetSetting(ApplicationSettingConstants.VacancySearchUrl);

            var searchUri = new Uri($"{baseUri}/{indexName}/_search");

            var node = searchUri;

            var settings = new ConnectionSettings(node);

            return new ElasticClient(settings);
        }

    }
}
