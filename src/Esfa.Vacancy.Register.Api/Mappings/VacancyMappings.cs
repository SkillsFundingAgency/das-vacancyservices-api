using AutoMapper;
using Esfa.Vacancy.Api.Types;
using Esfa.Vacancy.Register.Domain.Entities;

namespace Esfa.Vacancy.Register.Api.Mappings
{
    public class VacancyMappings : Profile
    {
        private const string UnknownText = "Unknown";

        public VacancyMappings()
        {
            CreateMap<Domain.Entities.Vacancy, Vacancy.Api.Types.Vacancy>()
                .ForMember(apiType => apiType.VacancyUrl, opt => opt.Ignore())
                .ForMember(apiType => apiType.Wage, opt => opt.Ignore())
                .ForMember(apiType => apiType.VacancyType, opt => opt.MapFrom(source => source.VacancyTypeId))
                .ForMember(apiType => apiType.WageUnit, opt => opt.MapFrom(source => source.WageUnitId))
                .ForMember(apiType => apiType.VacancyReference, opt => opt.MapFrom(source => source.VacancyReferenceNumber))
                .ForMember(apiType => apiType.LocationType, opt => opt.MapFrom(source => source.VacancyLocationTypeId))
                .AfterMap((src, dest) =>
                {
                    if (src.IsAnonymousEmployer && (VacancyStatus)src.VacancyStatusId == VacancyStatus.Live)
                    {
                        ApplyAnonymisationToVacancy(src, dest);
                    }

                    if (dest.VacancyType == VacancyType.Traineeship)
                    {
                        ResetWageFieldsForTraineeship(dest);
                    }
                    else
                    {
                        AdjustWageUnitBasedOnWageType(src, dest);

                        MapWageFields(src, dest);
                    }
                });
        }

        private void MapWageFields(Domain.Entities.Vacancy src, Vacancy.Api.Types.Vacancy dest)
        {
            switch (src.WageType)
            {
                case (int) WageType.LegacyText:
                    dest.Wage = UnknownText;
                    break;
                case (int) WageType.LegacyWeekly:
                case (int)WageType.Custom:
                    dest.Wage = src.WeeklyWage?.ToString("C0") ?? UnknownText;
                    break;
                case (int) WageType.ApprenticeshipMinimum:

                    break;
                case (int) WageType.NationalMinimum:

                    break;
                case (int) WageType.CustomRange:
                    dest.Wage = $"{src.WageLowerBound?.ToString("C0") ?? UnknownText} - {src.WageUpperBound?.ToString("C0") ?? UnknownText}";
                    break;
                case (int) WageType.CompetitiveSalary:
                    dest.Wage = "Competitive salary";
                    break;
                case (int) WageType.ToBeAgreedUponAppointment:
                    dest.Wage = "To be agreed upon appointment";
                    break;
                case (int) WageType.Unwaged:
                    dest.Wage = "Unwaged";
                    break;
            }
        }

        private void AdjustWageUnitBasedOnWageType(Domain.Entities.Vacancy src, Vacancy.Api.Types.Vacancy dest)
        {
            switch (src.WageType)
            {
                case (int) WageType.Custom:
                    break;
                case (int) WageType.CustomRange:
                    dest.WageUnit = src.WageUnitId.HasValue && (WageUnit) src.WageUnitId.Value == WageUnit.NotApplicable
                        ? WageUnit.Weekly
                        : dest.WageUnit;
                    break;
                case (int) WageType.LegacyWeekly:
                    dest.WageUnit = WageUnit.Weekly;
                    break;
                default:
                    dest.WageUnit = null;
                    break;
            }
        }

        private void ResetWageFieldsForTraineeship(Vacancy.Api.Types.Vacancy dest)
        {
            dest.Wage = null;
            dest.WageUnit = null;
            dest.HoursPerWeek = null;
        }

        private void ApplyAnonymisationToVacancy(Domain.Entities.Vacancy src, Vacancy.Api.Types.Vacancy dest)
        {
            dest.EmployerName = src.AnonymousEmployerName;
            dest.EmployerDescription = src.AnonymousEmployerDescription;
            dest.EmployerWebsite = null;

            dest.Location = new Vacancy.Api.Types.Address
            {
                Town = src.Location.Town
            };
        }
    }
}
