using AutoMapper;
using Esfa.Vacancy.Api.Types;
using Esfa.Vacancy.Register.Domain.Entities;

namespace Esfa.Vacancy.Register.Api.Mappings
{
    public class VacancyMappings : Profile
    {
        public VacancyMappings()
        {
            CreateMap<Domain.Entities.Vacancy, Vacancy.Api.Types.Vacancy>()
                .ForMember(apiType => apiType.VacancyUrl, opt => opt.Ignore())
                .ForMember(apiType => apiType.VacancyType, opt => opt.MapFrom(source => source.VacancyTypeId))
                .ForMember(apiType => apiType.WageUnit, opt => opt.MapFrom(source => source.WageUnitId))
                .ForMember(apiType => apiType.VacancyReference, opt => opt.MapFrom(source => source.VacancyReferenceNumber))
                .ForMember(apiType => apiType.LocationType, opt => opt.MapFrom(source => source.VacancyLocationTypeId))
                .AfterMap((src, dest) =>
                {
                    if (src.IsAnonymousEmployer && (Domain.Entities.VacancyStatus)src.VacancyStatusId == Domain.Entities.VacancyStatus.Live)
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

                        if (src.WeeklyWage.HasValue && src.WeeklyWage.Value > 0)
                        {
                            dest.WageText = src.WeeklyWage.Value.ToString("C0");
                        }
                        else
                        {
                            dest.WageText = "Unknown";
                        }
                    }
                });
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
            dest.WageText = null;
            dest.WageUnit = null;
            dest.HoursPerWeek = null;
        }

        private void ApplyAnonymisationToVacancy(Domain.Entities.Vacancy src, Vacancy.Api.Types.Vacancy dest)
        {
            dest.EmployerName = src.AnonymousEmployerName;
            dest.EmployerDescription = src.AnonymousEmployerDescription;
            dest.EmployerWebsite = null;

            dest.Location = new Vacancy.Api.Types.Address();
            dest.Location.Town = src.Location.Town;
        }
    }
}
