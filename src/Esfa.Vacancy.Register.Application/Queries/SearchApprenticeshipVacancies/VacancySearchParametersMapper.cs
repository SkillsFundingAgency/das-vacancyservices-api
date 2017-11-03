using System;
using System.Linq;
using Esfa.Vacancy.Register.Domain.Entities;

namespace Esfa.Vacancy.Register.Application.Queries.SearchApprenticeshipVacancies
{
    public static class VacancySearchParametersMapper
    {
        private const string NationwideLocationType = "Nationwide";

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
                    : null
            };
        }
    }
}
