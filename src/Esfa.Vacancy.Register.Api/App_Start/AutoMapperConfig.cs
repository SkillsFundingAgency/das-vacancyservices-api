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
                cfg.CreateMap<DomainTypes.Vacancy, ApiTypes.Vacancy>()
                    .ForMember(apiType => apiType.VacancyType, opt => opt.MapFrom(source => source.VacancyTypeId))
                    .ForMember(apiType => apiType.WageUnit, opt => opt.MapFrom(source => source.WageUnitId));
                cfg.CreateMap<DomainTypes.Address, ApiTypes.Address>();
            });
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
