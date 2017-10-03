using System;
using System.Linq;
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

        public async Task<SearchApprenticeshipVacanciesResponse> SearchApprenticeshipVacanciesAsync(
            VacancySearchParameters request)
        {
            var indexName = _provideSettings.GetSetting(ApplicationSettingKeyConstants.ApprenticeshipIndexAliasKey);

            var client = GetElasticSearchClient();

            ISearchResponse<ApprenticeshipSummary> esReponse;

            try
            {
                esReponse = await client.SearchAsync<ApprenticeshipSummary>(s => s
                    .Index(indexName)
                    .Type("apprenticeship")
                    .Query(q => q.Filtered(
                            sl => sl.Filter(
                                fs => fs.Terms(
                                    f => f.SubCategoryCode, request.StandardSectorCodes)))));
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
                TotalReturned = esReponse.Documents.Count(),
                ApprenticeshipSummaries = esReponse.Documents
            };

            return searchResponse;
        }

        private ElasticClient GetElasticSearchClient()
        {
            var baseUri = _provideSettings.GetSetting(ApplicationSettingKeyConstants.VacancySearchUrlKey);

            var node = new Uri(baseUri);

            var settings = new ConnectionSettings(node);

            return new ElasticClient(settings);
        }

    }
}
