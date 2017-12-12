using StructureMap;

namespace Esfa.Vacancy.Api.AcceptanceTests.DependencyResolution
{
    public static class IoC
    {
        public static IContainer Initialize()
        {
            return new Container(c =>
            {
                // Mock IVacancyRepository


            });
        }
    }
}
