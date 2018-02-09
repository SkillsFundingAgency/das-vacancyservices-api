using System;

namespace Esfa.Vacancy.Domain.Entities
{
    public class WageRange
    {
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }
        public decimal ApprenticeMinimumWage { get; set; }
    }
}