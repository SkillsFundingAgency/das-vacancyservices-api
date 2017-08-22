using AutoMapper;

namespace Esfa.Vacancy.Register.Api.Mappings
{
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
}
