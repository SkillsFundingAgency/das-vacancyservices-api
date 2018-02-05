using Esfa.Vacancy.Domain.Entities;

namespace Esfa.Vacancy.Application.Commands.CreateApprenticeship
{
    public interface IWageTypeMapper
    {
        LegacyWageType MapToLegacy(WageType originalWageType);
    }
}