using System;

namespace Esfa.Vacancy.Domain.Entities
{
    public class TrainingDetail
    {
        public string TrainingCode { get; set; }

        public int Level { get; set; }

        public DateTime? EffectiveTo { get; set; }

        public bool HasExpired => EffectiveTo > DateTime.Today;

        public string Title { get; set; }

        public string Uri { get; set; }

        public int FrameworkCode { get; set; }
    }
}