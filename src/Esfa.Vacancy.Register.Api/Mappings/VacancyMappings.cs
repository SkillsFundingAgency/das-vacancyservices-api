using AutoMapper;

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
                });
        }
        private static void ApplyAnonymisationToVacancy(Domain.Entities.Vacancy src, Vacancy.Api.Types.Vacancy dest)
        {
            dest.EmployerName = src.AnonymousEmployerName;
            dest.EmployerDescription = src.AnonymousEmployerDescription;
            dest.EmployerWebsite = null;

            dest.Location = new Vacancy.Api.Types.Address();
            dest.Location.Town = src.Location.Town;
        }
    }
}
