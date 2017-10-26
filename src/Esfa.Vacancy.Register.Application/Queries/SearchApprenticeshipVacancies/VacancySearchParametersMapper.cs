using System;
using System.Linq;
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
                FromDate = request.PostedInLastNumberOfDays > 0
                    ? DateTime.Today.AddDays(-request.PostedInLastNumberOfDays)
                    : (DateTime?)null,
                FrameworkCodes = request.FrameworkCodes,
                StandardIds = request.StandardCodes
            };
        }
    }
}
