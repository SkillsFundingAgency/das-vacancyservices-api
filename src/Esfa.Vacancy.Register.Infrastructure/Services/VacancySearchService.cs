using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Esfa.Vacancy.Register.Application.Interfaces;
using Esfa.Vacancy.Register.Application.Queries.SearchApprenticeshipVacancies;
using Esfa.Vacancy.Register.Domain.Entities;
using Esfa.Vacancy.Register.Infrastructure.Settings;
using Nest;
using SFA.DAS.NLog.Logger;

namespace Esfa.Vacancy.Register.Infrastructure.Services
{
    public class VacancySearchService : IVacancySearchService
    {
        private readonly IProvideSettings _provideSettings;
        private readonly ILog _logger;

        public VacancySearchService(IProvideSettings provideSettings, ILog logger)
        {
            _provideSettings = provideSettings;
            _logger = logger;
        }

        public async Task<List<ApprenticeshipSummary>> SearchApprenticeshipVacancies(
            SearchApprenticeshipVacanciesRequest request)
        {
            return new List<ApprenticeshipSummary>() {new ApprenticeshipSummary() {Title = "Hello World!"}};
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
