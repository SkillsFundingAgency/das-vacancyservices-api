using System.Collections.Generic;
using FluentValidation.Results;

namespace Esfa.Vacancy.Application.Queries.SearchApprenticeshipVacancies
{
    public class SubCategoryConversionResult
    {
        public List<string> SubCategoryCodes { get; set; } = new List<string>();
        public List<ValidationFailure> ValidationFailures { get; set; } = new List<ValidationFailure>();
    }
}