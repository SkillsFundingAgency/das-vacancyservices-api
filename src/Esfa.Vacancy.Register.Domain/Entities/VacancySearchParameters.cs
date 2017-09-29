using System.Collections.Generic;

namespace Esfa.Vacancy.Register.Domain.Entities
{
    public class VacancySearchParameters
    {
        public List<string> StandardSectorCodes { get; set; }

        public VacancySearchParameters()
        {
            StandardSectorCodes = new List<string>();
        }
    }
}
