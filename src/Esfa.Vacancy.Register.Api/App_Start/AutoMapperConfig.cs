using System.Reflection;
using AutoMapper;
using Esfa.Vacancy.Register.Api.Mappings;
using Esfa.Vacancy.Register.Application.Queries.SearchApprenticeshipVacancies;
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
                cfg.CreateMap<DomainTypes.ApprenticeshipSummary, ApiTypes.ApprenticeshipSummary>()
                    .ForMember(target => target.VacancyReference, c => c.MapFrom(source => int.Parse(source.VacancyReference)))
                    .ForMember(target => target.Longitude, c => c.MapFrom(source => source.Location == null ? (double?)null : source.Location.lon))
                    .ForMember(target => target.Latitude, c => c.MapFrom(source => source.Location == null ? (double?)null : source.Location.lat));
                cfg.CreateMap<SearchApprenticeshipVacanciesResponse, ApiTypes.SearchResponse<ApiTypes.ApprenticeshipSummary>>()
                    .ForMember(target => target.Results, c => c.MapFrom(source => source.ApprenticeshipSummaries));
                cfg.AddProfiles(Assembly.GetExecutingAssembly());
            });
        }
    }
}

