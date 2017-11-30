using System.Reflection;
using AutoMapper;
using Esfa.Vacancy.Application.Queries.SearchApprenticeshipVacancies;
using Esfa.Vacancy.Register.Api.Mappings;
using ApiTypes = Esfa.Vacancy.Api.Types;
using DomainTypes = Esfa.Vacancy.Register.Domain.Entities;

namespace Esfa.Vacancy.Register.Api
{
    public static class AutoMapperConfig
    {
        public static MapperConfiguration Configure()
        {
            return new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<int?, ApiTypes.WageUnit>().ConvertUsing(new IntToEnumConverter<ApiTypes.WageUnit>());
                cfg.CreateMap<int?, ApiTypes.TrainingType>().ConvertUsing(new IntToEnumConverter<ApiTypes.TrainingType>());
                cfg.CreateMap<ApiTypes.SearchApprenticeshipParameters, SearchApprenticeshipVacanciesRequest>()
                   .ForMember(target => target.StandardLarsCodes, config =>
                   {
                       config.Condition(source => !string.IsNullOrWhiteSpace(source.StandardLarsCodes));
                       config.MapFrom(source => source.StandardLarsCodes.Split(','));
                   })
                   .ForMember(target => target.FrameworkLarsCodes, config =>
                   {
                       config.Condition(source => !string.IsNullOrWhiteSpace(source.FrameworkLarsCodes));
                       config.MapFrom(source => source.FrameworkLarsCodes.Split(','));
                   });
                cfg.CreateMap<SearchApprenticeshipVacanciesResponse, ApiTypes.SearchResponse<ApiTypes.ApprenticeshipSummary>>()
                   .ForMember(target => target.Results, c => c.MapFrom(source => source.ApprenticeshipSummaries));
                cfg.CreateMap<DomainTypes.GeoPoint, ApiTypes.GeoPoint>()
                   .ForMember(target => target.Latitude, c => c.MapFrom(source => source.Lat))
                   .ForMember(target => target.Longitude, c => c.MapFrom(source => source.Lon));
                cfg.AddProfiles(Assembly.GetExecutingAssembly());
            });
        }
    }
}
