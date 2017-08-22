using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AutoMapper;
using Esfa.Vacancy.Register.Api.Mappings;
using ApiTypes = Esfa.Vacancy.Api.Types;

namespace Esfa.Vacancy.Register.Api.App_Start
{
    public static class AutoMapperConfig
    {
        public static void Configure()
        {
            var assignableMatchingTypes = 
                Assembly.GetExecutingAssembly().GetAssignableTypes(typeof(IObjectMappings)).ToList();
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<string, int?>().ConvertUsing(new StringToNullIntConverter());
                cfg.CreateMap<int, ApiTypes.VacancyType>().ConvertUsing(new IntToEnumConverter<ApiTypes.VacancyType>());
                cfg.CreateMap<int, ApiTypes.WageUnit>().ConvertUsing(new IntToEnumConverter<ApiTypes.WageUnit>());
                cfg.CreateMap<int, ApiTypes.VacancyLocationType>().ConvertUsing(new IntToEnumConverter<ApiTypes.VacancyLocationType>());

                assignableMatchingTypes.ForEach(a =>
                {
                    var m = (IObjectMappings)Activator.CreateInstance(a);
                    m.SetMappings(cfg);
                });
            });
        }

        private static IEnumerable<Type> GetAssignableTypes(this Assembly assembly, Type type)
        {
            return assembly.ExportedTypes
                .Where(t => !t.IsAbstract && !t.IsInterface && type.IsAssignableFrom(t));
        }
    }
}
