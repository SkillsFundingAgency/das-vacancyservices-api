using System.Collections.Generic;
using System.Threading.Tasks;
using Esfa.Vacancy.Register.Application.Interfaces;
using Esfa.Vacancy.Register.Domain.Entities;

namespace Esfa.Vacancy.Register.Application.Queries.SearchApprenticeshipVacancies
{
    public class VacancySearchParametersConverter : IVacancySearchParametersConverter
    {
        private readonly IStandardCodeConverter _standardCodeConverter;
        private readonly IFrameworkCodeConverter _frameworkCodeConverter;

        public VacancySearchParametersConverter(IStandardCodeConverter standardCodeConverter, IFrameworkCodeConverter frameworkCodeConverter)
        {
            _standardCodeConverter = standardCodeConverter;
            _frameworkCodeConverter = frameworkCodeConverter;
        }

        public async Task<VacancySearchParameters> ConvertFrom(SearchApprenticeshipVacanciesRequest request)
        {
            var combinedSubCategoryCodes = new List<string>();
            combinedSubCategoryCodes.AddRange(await _standardCodeConverter.ConvertAsync(request.StandardCodes));
            combinedSubCategoryCodes.AddRange(await _frameworkCodeConverter.ConvertAsync(request.FrameworkCodes));

            return new VacancySearchParameters
            {
                PageNumber = request.PageNumber,
                PageSize = request.PageSize,
                SubCategoryCodes = combinedSubCategoryCodes
            };
        }
    }
}
