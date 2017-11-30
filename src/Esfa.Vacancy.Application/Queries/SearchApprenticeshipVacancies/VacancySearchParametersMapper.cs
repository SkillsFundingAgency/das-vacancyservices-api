using System;
using System.Linq;
using Esfa.Vacancy.Domain.Entities;

namespace Esfa.Vacancy.Application.Queries.SearchApprenticeshipVacancies
{
    public static class VacancySearchParametersMapper
    {
        public const string NationwideLocationType = "National";
        public const string NonNationwideLocationType = "NonNational";

        public static VacancySearchParameters Convert(SearchApprenticeshipVacanciesRequest request)
        {
            return new VacancySearchParameters
            {
                PageNumber = request.PageNumber,
                PageSize = request.PageSize,
                FromDate = request.PostedInLastNumberOfDays.HasValue
                    ? DateTime.Today.AddDays(-request.PostedInLastNumberOfDays.Value)
                    : (DateTime?)null,
                FrameworkLarsCodes = request.FrameworkLarsCodes.Select(x => x.Trim()).ToList(),
                StandardLarsCodes = request.StandardLarsCodes.Select(x => x.Trim()).ToList(),
                LocationType = request.NationwideOnly
                    ? NationwideLocationType
                    : NonNationwideLocationType,
                Longitude = request.NationwideOnly ? null : request.Longitude,
                Latitude = request.NationwideOnly ? null : request.Latitude,
                DistanceInMiles = request.NationwideOnly ? null : request.DistanceInMiles
            };
        }
    }
}
