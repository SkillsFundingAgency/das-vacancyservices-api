using System;
using System.Net;
using System.Threading.Tasks;
using Esfa.Vacancy.Register.Application.Interfaces;
using Esfa.Vacancy.Register.Application.Queries.SearchApprenticeshipVacancies;
using Esfa.Vacancy.Register.Domain.Entities;
using Esfa.Vacancy.Register.Infrastructure.Exceptions;
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

        public async Task<SearchApprenticeshipVacanciesResponse> SearchApprenticeshipVacancies(
            SearchApprenticeshipVacanciesRequest request)
        {
            var indexName = _provideSettings.GetSetting(ApplicationSettingConstants.ApprenticeshipIndexAlias);

            var client = GetElasticSearchClient();

            ISearchResponse<ApprenticeshipSummary> esReponse;

            try
            {
                esReponse = await client.SearchAsync<ApprenticeshipSummary>(s => s
                    .Index(indexName)
                    .Type("apprenticeship")
                    .From(0)
                    .Size(5)
                    .Query(q => q.Filtered(
                            sl => sl.Filter(
                                fs => fs.Terms(
                                    f => f.SubCategoryCode, request.StandardCodes)))));
            }
            catch (WebException e)
            {
                throw new InfrastructureException(e);
            }

            if (!esReponse.ConnectionStatus.Success)
            {
                var ex = new Exception("Unexpected response received from Elastic Search");
                throw new InfrastructureException(ex);
            }

            var searchResponse = new SearchApprenticeshipVacanciesResponse()
            {
                TotalMatched = esReponse.Total,
                TotalReturned = esReponse.HitsMetaData.Total,
                ApprenticeshipSummaries = esReponse.Documents
            };

            return searchResponse;
        }

        private ElasticClient GetElasticSearchClient()
        {
            var baseUri = _provideSettings.GetSetting(ApplicationSettingConstants.VacancySearchUrl);

            var node = new Uri(baseUri);

            var settings = new ConnectionSettings(node);

            return new ElasticClient(settings);
        }

    }
}
