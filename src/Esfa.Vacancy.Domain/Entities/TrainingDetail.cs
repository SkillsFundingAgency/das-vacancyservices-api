using System;

namespace Esfa.Vacancy.Domain.Entities
{
    public class TrainingDetail
    {
        public int Level { get; set; }

        public DateTime? EffectiveTo { get; set; }

        public bool HasExpired => EffectiveTo > DateTime.Today;
    }
}