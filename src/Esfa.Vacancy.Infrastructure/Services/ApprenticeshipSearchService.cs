using System;
using System.Linq;
using System.Threading.Tasks;
using Esfa.Vacancy.Application.Interfaces;
using Esfa.Vacancy.Application.Queries.SearchApprenticeshipVacancies;
using Esfa.Vacancy.Domain.Entities;
using Esfa.Vacancy.Domain.Interfaces;
using Esfa.Vacancy.Infrastructure.Exceptions;
using SFA.DAS.NLog.Logger;
using SFA.DAS.VacancyServices.Search;
using SFA.DAS.VacancyServices.Search.Entities;
using SFA.DAS.VacancyServices.Search.Requests;
using SFA.DAS.VacancyServices.Search.Responses;

namespace Esfa.Vacancy.Infrastructure.Services
{
    public class ApprenticeshipSearchService : IApprenticeshipSearchService
    {
        private readonly ILog _logger;
        private readonly IApprenticeshipSearchClient _apprenticeshipSearchClient;

        public ApprenticeshipSearchService(ILog logger, IApprenticeshipSearchClient apprenticeshipSearchClient)
        {
            _logger = logger;
            _apprenticeshipSearchClient = apprenticeshipSearchClient;
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

            _logger.Info($"Searching apprenticeships with following parameters: {parameters}");

            var searchClientParameters = GetSearchClientParameters(parameters);

            ApprenticeshipSearchResponse searchClientResponse;
            try
            {
                searchClientResponse = _apprenticeshipSearchClient.Search(searchClientParameters);
            }
            catch (Exception e)
            {
                throw new InfrastructureException(e);
            }
            
            var searchResponse = new SearchApprenticeshipVacanciesResponse()
            {
                TotalMatched = searchClientResponse.Total,
                TotalReturned = searchClientResponse.Results.Count(),
                TotalPages = searchClientResponse.TotalPages,
                CurrentPage = parameters.PageNumber,
                SortBy = parameters.SortBy,
                ApprenticeshipSummaries = searchClientResponse.Results.Select(GetApprenticeshipSummary)
            };

            return searchResponse;
        }

        private ApprenticeshipSearchRequestParameters GetSearchClientParameters(VacancySearchParameters parameters)
        {
            var searchClientParameters = new ApprenticeshipSearchRequestParameters
            {
                FrameworkLarsCodes = parameters.FrameworkLarsCodes,
                StandardLarsCodes = parameters.StandardLarsCodes,
                PageSize = parameters.PageSize,
                PageNumber = parameters.PageNumber,
                FromDate = parameters.FromDate,
                VacancyLocationType = string.IsNullOrEmpty(parameters.LocationType) ? VacancyLocationType.Unknown : (VacancyLocationType)Enum.Parse(typeof(VacancyLocationType), parameters.LocationType),
                Longitude = parameters.Longitude,
                Latitude = parameters.Latitude,
                SearchRadius = parameters.DistanceInMiles,
                CalculateSubCategoryAggregations = false,
                ProviderUkprn = parameters.ProviderUkprn
            };

            switch (parameters.SortBy)
            {
                case SortBy.Age:
                    searchClientParameters.SortType = VacancySearchSortType.RecentlyAdded;
                    break;
                case SortBy.ExpectedStartDate:
                    searchClientParameters.SortType = VacancySearchSortType.ExpectedStartDate;
                    break;
                case SortBy.Distance:
                    searchClientParameters.SortType = VacancySearchSortType.Distance;
                    break;
                default:
                    searchClientParameters.SortType = VacancySearchSortType.Relevancy;
                    break;
            }

            return searchClientParameters;
        }

        private ApprenticeshipSummary GetApprenticeshipSummary(ApprenticeshipSearchResult result)
        {
            return new ApprenticeshipSummary
            {
                DistanceInMiles = result.Distance,
                AnonymousEmployerName = result.AnonymousEmployerName,
                ApprenticeshipLevel = result.ApprenticeshipLevel.ToString(),
                Category = result.Category,
                CategoryCode = result.CategoryCode,
                ClosingDate = result.ClosingDate,
                Description = result.Description,
                EmployerName = result.EmployerName,
                FrameworkLarsCode = result.FrameworkLarsCode,
                HoursPerWeek = result.HoursPerWeek,
                Id = result.Id,
                IsDisabilityConfident = result.IsDisabilityConfident,
                IsEmployerAnonymous = result.IsEmployerAnonymous,
                IsPositiveAboutDisability = result.IsPositiveAboutDisability,
                Location = new Domain.Entities.GeoPoint {Lat = result.Location.lat, Lon = result.Location.lon},
                NumberOfPositions = result.NumberOfPositions,
                PostedDate = result.PostedDate,
                ProviderName = result.ProviderName,
                ProviderUkprn = result.ProviderUkprn,
                StandardLarsCode = result.StandardLarsCode,
                StartDate = result.StartDate,
                SubCategory = result.SubCategory,
                SubCategoryCode = result.SubCategoryCode,
                Title = result.Title,
                VacancyLocationType = result.VacancyLocationType.ToString(),
                VacancyReference = result.VacancyReference,
                WageAmount = result.WageAmount,
                WageAmountLowerBound = result.WageAmountLowerBound,
                WageAmountUpperBound = result.WageAmountUpperBound,
                WageText = result.WageText,
                WageType = result.WageType,
                WageUnit = result.WageUnit,
                WorkingWeek = result.WorkingWeek
            };
        }
    }
}
