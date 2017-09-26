using System.Reflection;
using AutoMapper;
using Esfa.Vacancy.Register.Api.Mappings;
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
                cfg.CreateMap<int?, ApiTypes.VacancyType>().ConvertUsing(new IntToEnumConverter<ApiTypes.VacancyType>());
                cfg.CreateMap<int?, ApiTypes.WageUnit>().ConvertUsing(new IntToEnumConverter<ApiTypes.WageUnit>());
                cfg.CreateMap<int?, ApiTypes.VacancyLocationType>().ConvertUsing(new IntToEnumConverter<ApiTypes.VacancyLocationType>());
                cfg.CreateMap<int?, ApiTypes.TrainingType>().ConvertUsing(new IntToEnumConverter<ApiTypes.TrainingType>());
                cfg.CreateMap<DomainTypes.Address, ApiTypes.Address>();
                cfg.AddProfiles(Assembly.GetExecutingAssembly());
            });
        }
    }
}

