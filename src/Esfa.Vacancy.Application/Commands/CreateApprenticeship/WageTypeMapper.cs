﻿using System;
using Esfa.Vacancy.Domain.Entities;

namespace Esfa.Vacancy.Application.Commands.CreateApprenticeship
{
    public class WageTypeMapper : IWageTypeMapper
    {
        public LegacyWageType MapToLegacy(WageType originalWageType)
        {
            switch (originalWageType)
            {
                case WageType.Custom:
                    return LegacyWageType.Custom;
                case WageType.NationalMinimumWage:
                    return LegacyWageType.NationalMinimum;
                case WageType.ApprenticeshipMinimumWage:
                    return LegacyWageType.ApprenticeshipMinimum;
                case WageType.Unwaged:
                    return LegacyWageType.Unwaged;
                case WageType.CompetitiveSalary:
                    return LegacyWageType.CompetitiveSalary;
                case WageType.ToBeSpecified:
                    return LegacyWageType.ToBeAgreedUponAppointment;
                default:
                    throw new ArgumentOutOfRangeException(nameof(originalWageType), originalWageType, null);
            }
        }
    }
}