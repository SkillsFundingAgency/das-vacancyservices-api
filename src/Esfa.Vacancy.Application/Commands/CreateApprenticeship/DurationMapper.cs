using System;
using Esfa.Vacancy.Domain.Entities;

namespace Esfa.Vacancy.Application.Commands.CreateApprenticeship
{
    public class DurationMapper : IDurationMapper
    {
        public DomainDurationType MapTypeToDomainType(DurationType originalDurationType)
        {
            switch (originalDurationType)
            {
                case DurationType.Weeks:
                    return DomainDurationType.Weeks;
                case DurationType.Months:
                    return DomainDurationType.Months;
                case DurationType.Years:
                    return DomainDurationType.Years;
                default:
                    throw new ArgumentOutOfRangeException(nameof(originalDurationType), originalDurationType, null);
            }
        }
    }
}