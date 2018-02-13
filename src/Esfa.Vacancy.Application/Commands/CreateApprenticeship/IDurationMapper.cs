﻿using Esfa.Vacancy.Domain.Entities;

namespace Esfa.Vacancy.Application.Commands.CreateApprenticeship
{
    public interface IDurationMapper
    {
        DomainDurationType MapTypeToDomainType(DurationType originalDurationType);

        string MapToDisplayText(DurationType durationType, int durationValue);
    }
}
