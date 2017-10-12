using System.Collections.Generic;
using FluentValidation.Results;

namespace Esfa.Vacancy.Register.Application.Queries.SearchApprenticeshipVacancies
{
    public class SubCategoryConversionResult
    {
        public List<string> SubCategoryCodes { get; set; } = new List<string>();
        public List<ValidationResult> ValidationResults { get; set; } = new List<ValidationResult>();
    }
}