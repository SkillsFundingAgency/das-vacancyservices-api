using System;
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
                cfg.CreateMap<string, int?>().ConvertUsing(new StringToNullIntConverter());
                cfg.CreateMap<int, ApiTypes.VacancyType>().ConvertUsing(new IntToEnumConverter<ApiTypes.VacancyType>());
                cfg.CreateMap<int, ApiTypes.WageUnit>().ConvertUsing(new IntToEnumConverter<ApiTypes.WageUnit>());
                cfg.CreateMap<int, ApiTypes.VacancyLocationType>().ConvertUsing(new IntToEnumConverter<ApiTypes.VacancyLocationType>());

                CreateVacancyDetailsMapping(cfg);

                cfg.CreateMap<DomainTypes.Address, ApiTypes.Address>();
            });
        }

        private static void CreateVacancyDetailsMapping(IMapperConfigurationExpression cfg)
        {
            cfg.CreateMap<DomainTypes.Vacancy, ApiTypes.Vacancy>()
                .ForMember(apiType => apiType.VacancyType, opt => opt.MapFrom(source => source.VacancyTypeId))
                .ForMember(apiType => apiType.WageUnit, opt => opt.MapFrom(source => source.WageUnitId))
                .ForMember(apiType => apiType.VacancyReference, opt => opt.MapFrom(source => source.VacancyReferenceNumber))
                .ForMember(apiType => apiType.VacancyLocationType, opt => opt.MapFrom(source => source.VacancyLocationTypeId))
                .AfterMap((src, dest) =>
                    {
                        if (src.IsAnonymousEmployer && (DomainTypes.VacancyStatus)src.VacancyStatusId == DomainTypes.VacancyStatus.Live)
                        {
                            ApplyAnonymisationToVacancy(src, dest);
                        }
                    });
        }

        private static void ApplyAnonymisationToVacancy(DomainTypes.Vacancy src, ApiTypes.Vacancy dest)
        {
            dest.EmployerName = src.AnonymousEmployerName;
            dest.EmployerDescription = src.AnonymousEmployerDescription;
            dest.EmployerWebsite = null;

            dest.VacancyLocation.AddressLine1 = null;
            dest.VacancyLocation.AddressLine2 = null;
            dest.VacancyLocation.AddressLine3 = null;
            dest.VacancyLocation.AddressLine4 = null;
            dest.VacancyLocation.AddressLine5 = null;
            dest.VacancyLocation.PostCode = null;
            dest.VacancyLocation.Longitude = null;
            dest.VacancyLocation.Latitude = null;
        }
    }

    public class StringToNullIntConverter : ITypeConverter<string, int?>
    {
        public int? Convert(string source, int? destination, ResolutionContext context)
        {
            int value;
            if ((string.IsNullOrWhiteSpace(source)) || (!int.TryParse(source, out value)))
                return null;
            return value;
        }
    }

    public class IntToEnumConverter<T> : ITypeConverter<int, T>
    {
        public T Convert(int source, T destination, ResolutionContext context)
        {
            if (!typeof(T).IsEnum) throw new Exception("Only Enum types are allowed.");
            if (source != null)
            {
                var name = Enum.GetName(typeof(T), source);
                if (name != null)
                {
                    return (T)Enum.Parse(typeof(T), name);
                }
            }

            var en = Enum.GetValues(typeof(T)).GetEnumerator();
            en.MoveNext();
            return (T)en.Current;
        }
    }
}
