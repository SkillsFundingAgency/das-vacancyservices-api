using System;
using AutoMapper;

namespace Esfa.Vacancy.Register.Api.Mappings
{
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
