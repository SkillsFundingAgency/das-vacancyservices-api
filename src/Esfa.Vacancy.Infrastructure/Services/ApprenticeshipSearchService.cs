using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Esfa.Vacancy.Application.Interfaces;
using Esfa.Vacancy.Application.Queries.SearchApprenticeshipVacancies;
using Esfa.Vacancy.Domain.Constants;
using Esfa.Vacancy.Domain.Entities;
using Esfa.Vacancy.Domain.Interfaces;
using Esfa.Vacancy.Infrastructure.Exceptions;
using Nest;
using SFA.DAS.NLog.Logger;

namespace Esfa.Vacancy.Infrastructure.Services
{
    public class ApprenticeshipSearchService : IApprenticeshipSearchService
    {
        private readonly IProvideSettings _provideSettings;
        private readonly ILog _logger;
        private readonly IElasticClient _elasticClient;
        private readonly IGeoSearchResultDistanceSetter _distanceSetter;

        public ApprenticeshipSearchService(IProvideSettings provideSettings, ILog logger, IElasticClient elasticClient, IGeoSearchResultDistanceSetter distanceSetter)
        {
            _provideSettings = provideSettings;
            _logger = logger;
            _elasticClient = elasticClient;
            _distanceSetter = distanceSetter;
        }

        public Task<SearchApprenticeshipVacanciesResponse> SearchApprenticeshipVacanciesAsync(
            VacancySearchParameters parameters)
        {
            var retry = PollyRetryPolicies.GetFixedIntervalPolicy((exception, time, retryCount, context) =>
            {
                _logger.Warn($"Error searching for apprenticeships in search index: ({exception.Message}). Retrying... attempt {retryCount}");
            });

            return retry.ExecuteAsync(() => InternalSearchApprenticeshipVacanciesAsync(parameters));
        }

        private async Task<SearchApprenticeshipVacanciesResponse> InternalSearchApprenticeshipVacanciesAsync(
            VacancySearchParameters parameters)
        {
            var indexName = _provideSettings.GetSetting(ApplicationSettingKeys.ApprenticeshipIndexAliasKey);

            ISearchResponse<ApprenticeshipSummary> esReponse;

            _logger.Info($"Querying Apprenticeship Elastic index with following parameters: {parameters}");

            try
            {
                esReponse = await _elasticClient.SearchAsync<ApprenticeshipSummary>(search =>
                {
                    search.Index(indexName)
                        .Type("apprenticeship")
                        .Skip((parameters.PageNumber - 1) * parameters.PageSize)
                        .Take(parameters.PageSize)
                        .Query(query =>
                        {
                            var container = new QueryContainer();
                            if (parameters.FrameworkLarsCodes.Any() || parameters.StandardLarsCodes.Any())
                            {
                                container &=
                                    query.Terms(qt => qt
                                        .Field(apprenticeship => apprenticeship.FrameworkLarsCode)
                                        .Terms(parameters.FrameworkLarsCodes))
                                    || query.Terms(qt => qt
                                        .Field(apprenticeship => apprenticeship.StandardLarsCode)
                                        .Terms(parameters.StandardLarsCodes));
                            }

                            if (parameters.FromDate.HasValue)
                            {
                                container &= query.DateRange(range => range
                                    .Field(apprenticeship => apprenticeship.PostedDate)
                                    .GreaterThanOrEquals(DateMath.Anchored(parameters.FromDate.GetValueOrDefault())));
                            }

                            if (parameters.LocationType == VacancySearchParametersMapper.NationwideLocationType)
                            {
                                container &= query.Match(m => m
                                    .Field(apprenticeship => apprenticeship.VacancyLocationType)
                                    .Query(parameters.LocationType));
                            }

                            if (parameters.HasGeoSearchFields)
                            {
                                container &=
                                    query.GeoDistance(gd => gd
                                        .Field(summary => summary.Location)
                                        .Location(parameters.Latitude.GetValueOrDefault(), parameters.Longitude.GetValueOrDefault())
                                        .Distance(parameters.DistanceInMiles.GetValueOrDefault(), DistanceUnit.Miles));
                            }

                            return container;
                        });

                    switch (parameters.SortBy)
                    {
                        case SortBy.Distance:
                            search.TrySortByDistance(parameters);
                            search.SortByAge();
                            break;

                        case SortBy.ExpectedStartDate:
                            search.SortByExpectedStartDate();
                            search.TrySortByDistance(parameters);
                            break;

                        default:
                            search.SortByAge();
                            search.TrySortByDistance(parameters);
                            break;
                    }

                    return search;
                }).ConfigureAwait(false);

                _logger.Info($"Retrieved {esReponse.Total} apprenticeships from Elastic search with parameters {parameters}");
            }
            catch (WebException e)
            {
                throw new InfrastructureException(e);
            }

            if (!esReponse.IsValid)
            {
                var ex = new Exception($"Unexpected response received from Elastic Search: { esReponse.DebugInformation}");
                throw new InfrastructureException(ex);
            }

            if (parameters.HasGeoSearchFields)
            {
                _distanceSetter.SetDistance(parameters, esReponse);
            }

            var searchResponse = new SearchApprenticeshipVacanciesResponse()
            {
                TotalMatched = esReponse.Total,
                TotalReturned = esReponse.Documents.Count(),
                CurrentPage = parameters.PageNumber,
                TotalPages = Math.Ceiling((double)esReponse.Total / parameters.PageSize),
                SortBy = parameters.SortBy,
                ApprenticeshipSummaries = esReponse.Documents
            };

            return searchResponse;
        }
    }
}
