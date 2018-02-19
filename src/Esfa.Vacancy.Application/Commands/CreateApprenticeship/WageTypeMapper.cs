using System;
using Esfa.Vacancy.Domain.Entities;

namespace Esfa.Vacancy.Application.Commands.CreateApprenticeship
{
    public class WageTypeMapper : IWageTypeMapper
    {
        public LegacyWageType MapToLegacy(CreateApprenticeshipRequest request)
        {
            switch (request.WageType)
            {
                case WageType.Custom:
                    if (request.MinWage.HasValue && request.MaxWage.HasValue && request.MaxWage > request.MinWage)
                        return LegacyWageType.CustomRange;
                    else
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
                    throw new ArgumentOutOfRangeException(nameof(request.WageType), request.WageType, null);
            }
        }
    }
}