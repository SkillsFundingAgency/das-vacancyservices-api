using System.Reflection;
using AutoMapper;
using Esfa.Vacancy.Register.Api.Mappings;
using Esfa.Vacancy.Register.Application.Queries.SearchApprenticeshipVacancies;
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
                cfg.CreateMap<int?, ApiTypes.VacancyType>().ConvertUsing(new IntToEnumConverter<ApiTypes.VacancyType>());
                cfg.CreateMap<int?, ApiTypes.WageUnit>().ConvertUsing(new IntToEnumConverter<ApiTypes.WageUnit>());
                cfg.CreateMap<int?, ApiTypes.VacancyLocationType>().ConvertUsing(new IntToEnumConverter<ApiTypes.VacancyLocationType>());
                cfg.CreateMap<int?, ApiTypes.TrainingType>().ConvertUsing(new IntToEnumConverter<ApiTypes.TrainingType>());
                cfg.CreateMap<DomainTypes.Address, ApiTypes.Address>();
                cfg.CreateMap<ApiTypes.SearchApprenticeshipParameters, SearchApprenticeshipVacanciesRequest>()
                    .ForMember(target =>
                        target.StandardCodes, c
                        => c.MapFrom(source =>
                            string.IsNullOrWhiteSpace(source.StandardCodes) ? null : source.StandardCodes.Split(',')))
                    .ForMember(target =>
                        target.FrameworkCodes, c
                        => c.MapFrom(source =>
                            string.IsNullOrWhiteSpace(source.FrameworkCodes) ? null : source.FrameworkCodes.Split(',')));
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

