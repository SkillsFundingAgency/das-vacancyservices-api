﻿using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Esfa.Vacancy.Application.Interfaces;
using Esfa.Vacancy.Application.Queries.SearchApprenticeshipVacancies;
using Esfa.Vacancy.Domain.Entities;
using Esfa.Vacancy.Infrastructure.Exceptions;
using Esfa.Vacancy.Infrastructure.Settings;
using Nest;
using SFA.DAS.NLog.Logger;

namespace Esfa.Vacancy.Infrastructure.Services
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
            var retry = PollyRetryPolicies.GetFixedIntervalPolicy((exception, time, retryCount, context) =>
            {
                _logger.Warn($"Error searching for apprenticeships in search index: ({exception.Message}). Retrying... attempt {retryCount}");
            });

            return await retry.ExecuteAsync(() => InternalSearchApprenticeshipVacanciesAsync(parameters));
        }

        private async Task<SearchApprenticeshipVacanciesResponse> InternalSearchApprenticeshipVacanciesAsync(
            VacancySearchParameters parameters)
        {
            var indexName = _provideSettings.GetSetting(ApplicationSettingKeyConstants.ApprenticeshipIndexAliasKey);

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
                            var container =  
                                (query.Terms(apprenticeship => apprenticeship.FrameworkLarsCode, parameters.FrameworkLarsCodes)
                                 || query.Terms(apprenticeship => apprenticeship.StandardLarsCode, parameters.StandardLarsCodes))
                                    && query.Match(m => m.OnField(apprenticeship => apprenticeship.VacancyLocationType).Query(parameters.LocationType))
                                    && query.Range(range =>
                                        range.OnField(apprenticeship => apprenticeship.PostedDate)
                                            .GreaterOrEquals(parameters.FromDate));

                            if (parameters.HasGeoSearchFields)
                            {
                                container = container && query.Filtered(descriptor =>
                                    descriptor.Filter(filterDescriptor =>
                                        filterDescriptor.GeoDistance(summary => summary.Location, distanceFilterDescriptor =>
                                            distanceFilterDescriptor.Location(parameters.Latitude.Value, parameters.Longitude.Value)
                                                .Distance(parameters.DistanceInMiles.Value, GeoUnit.Miles))));
                            }

                            return container;
                        });

                    if (parameters.HasGeoSearchFields)
                    {
                        search.SortGeoDistance(descriptor =>
                            descriptor.PinTo(parameters.Latitude.Value, parameters.Longitude.Value)
                                .Unit(GeoUnit.Miles)
                                .OnField(summary => summary.Location));
                    }
                    else
                    {
                        search.SortDescending(summary => summary.PostedDate);
                    }

                    return search;
                });

                _logger.Info($"Retrieved {esReponse.Total} apprenticeships from Elastic search with parameters {parameters}");
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
