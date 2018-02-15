using System;
using Esfa.Vacancy.Domain.Entities;
using Humanizer;

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

        public string MapToDisplayText(DurationType durationType, int durationValue)
        {
            switch (durationType)
            {
                case DurationType.Weeks:
                    return "weeks".ToQuantity(durationValue);
                case DurationType.Months:
                    return "months".ToQuantity(durationValue);
                case DurationType.Years:
                    return "years".ToQuantity(durationValue);
                default:
                    return String.Empty;
            }
        }
    }
}