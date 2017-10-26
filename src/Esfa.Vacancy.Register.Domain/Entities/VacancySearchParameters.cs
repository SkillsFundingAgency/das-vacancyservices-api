using System;
using System.Collections.Generic;

namespace Esfa.Vacancy.Register.Domain.Entities
{
    public class VacancySearchParameters
    {
        public List<string> FrameworkCodes { get; set; } = new List<string>();
        public List<string> StandardIds { get; set; } = new List<string>();
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
        public DateTime? FromDate { get; set; }
    }
}
