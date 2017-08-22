using AutoMapper;
using Esfa.Vacancy.Register.Api.App_Start;
using ApiTypes = Esfa.Vacancy.Api.Types;
using DomainTypes = Esfa.Vacancy.Register.Domain.Entities;

namespace Esfa.Vacancy.Register.Api.Mappings
{
    public class AddressMappings : IObjectMappings
    {
        public void SetMappings(IMapperConfigurationExpression cfg)
        {
            cfg.CreateMap<DomainTypes.Address, ApiTypes.Address>();
        }
    }
}
