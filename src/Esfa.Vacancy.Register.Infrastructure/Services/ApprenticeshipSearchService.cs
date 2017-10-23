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
    public class ApprenticeshipSearchService : IApprenticeshipSearchService
    {
        private readonly IProvideSettings _provideSettings;
        private readonly ILog _logger;
        private readonly IElasticClient _elasticClient;

        public ApprenticeshipSearchService(IProvideSettings provideSettings, ILog logger, IElasticClient elasticClient)
        {
            _provideSettings = provideSettings;
            _logger = logger;
            _elasticClient = elasticClient;
        }

        public async Task<SearchApprenticeshipVacanciesResponse> SearchApprenticeshipVacanciesAsync(
            VacancySearchParameters parameters)
        {
            var retry = VacancyRegisterRetryPolicy.GetFixedIntervalPolicy((exception, time, retryCount, context) =>
            {
                _logger.Warn($"Error searching for vacancies in search index: ({exception.Message}). Retrying... attempt {retryCount}");
            });

            return await retry.ExecuteAsync(() => InternalSearchApprenticeshipVacanciesAsync(parameters));
        }

        private async Task<SearchApprenticeshipVacanciesResponse> InternalSearchApprenticeshipVacanciesAsync(
            VacancySearchParameters parameters)
        {
            var indexName = _provideSettings.GetSetting(ApplicationSettingKeyConstants.ApprenticeshipIndexAliasKey);

            ISearchResponse<ApprenticeshipSummary> esReponse;

            try
            {
                esReponse = await _elasticClient.SearchAsync<ApprenticeshipSummary>(s => s
                    .Index(indexName)
                    .Type("apprenticeship")
                    .Skip((parameters.PageNumber - 1) * parameters.PageSize)
                    .Take(parameters.PageSize)
                    .Query(query =>
                        query.Terms(f => f.SubCategoryCode, parameters.SubCategoryCodes)
                        && query.Range(p => p.OnField(r => r.PostedDate).GreaterOrEquals(parameters.FromDate))
                    ));
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
                CurrentPage = parameters.PageNumber,
                TotalPages = Math.Ceiling((double)esReponse.Total / parameters.PageSize),
                ApprenticeshipSummaries = esReponse.Documents
            };

            return searchResponse;
        }
    }
}
