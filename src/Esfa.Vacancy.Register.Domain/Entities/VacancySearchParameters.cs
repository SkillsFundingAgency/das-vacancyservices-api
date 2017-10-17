﻿using System;
using System.Collections.Generic;

namespace Esfa.Vacancy.Register.Domain.Entities
{
    public class VacancySearchParameters
    {
        public List<string> SubCategoryCodes { get; set; } = new List<string>();
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
        public int PostedInDays { get; set; }

        public DateTime? FromDate => PostedInDays > 0 ? DateTime.Today.AddDays(-PostedInDays) : (DateTime?)null;
    }
}
