using AutoMapper;
using ApiTypes = Esfa.Vacancy.Api.Types;
using DomainTypes = Esfa.Vacancy.Register.Domain.Entities;

namespace Esfa.Vacancy.Register.Api.Mappings
{
    public class VacancyMappings : IObjectMappings
    {
        public void SetMappings(IMapperConfigurationExpression cfg)
        {
            cfg.CreateMap<DomainTypes.Vacancy, ApiTypes.Vacancy>()
                .ForMember(apiType => apiType.VacancyType, opt => opt.MapFrom(source => source.VacancyTypeId))
                .ForMember(apiType => apiType.WageUnit, opt => opt.MapFrom(source => source.WageUnitId))
                .ForMember(apiType => apiType.VacancyLocationType, opt => opt.MapFrom(source => source.VacancyLocationTypeId))
                .AfterMap(AfterMap);
        }

        private void AfterMap(DomainTypes.Vacancy source, ApiTypes.Vacancy target)
        {

        }
    }
}
