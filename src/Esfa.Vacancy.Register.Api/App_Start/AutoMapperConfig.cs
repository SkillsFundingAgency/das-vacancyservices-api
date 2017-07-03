using AutoMapper;
using ApiTypes = Esfa.Vacancy.Api.Types;
using DomainTypes = Esfa.Vacancy.Register.Domain.Entities;

namespace Esfa.Vacancy.Register.Api.App_Start
{
    public static class AutoMapperConfig
    {
        public static void Configure()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<DomainTypes.Vacancy, ApiTypes.Vacancy>()
                .ForMember(apiType => apiType.VacancyType, opt => opt.MapFrom(source => (ApiTypes.VacancyType)source.VacancyTypeId))
                .ForMember(apiType => apiType.WageUnit, opt => opt.MapFrom(source => (ApiTypes.WageUnit)source.WageUnitId));
            });
        }
    }
}
