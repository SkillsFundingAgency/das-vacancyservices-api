using System;
using Esfa.Vacancy.Register.Domain.Entities;

namespace Esfa.Vacancy.Register.Application.Queries.SearchApprenticeshipVacancies
{
    public class VacancySearchParametersMapper
    {
        public static VacancySearchParameters Convert(SearchApprenticeshipVacanciesRequest request)
        {
            return new VacancySearchParameters()
            {
                PageNumber = request.PageNumber,
                PageSize = request.PageSize,
                FromDate = request.PostedInLastNumberOfDays.HasValue
                    ? DateTime.Today.AddDays(-request.PostedInLastNumberOfDays.Value)
                    : (DateTime?)null,
                FrameworkCodes = request.FrameworkCodes,
                StandardIds = request.StandardCodes
            };
        }
    }
}
