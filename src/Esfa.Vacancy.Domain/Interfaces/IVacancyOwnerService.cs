using System.Threading.Tasks;

namespace Esfa.Vacancy.Domain.Interfaces
{
    public interface IVacancyOwnerService
    {
        Task<int?> GetVacancyOwnerLinkIdAsync(int providerUkprn, int providerSiteEdsUrn, int employerEdsUrn);
    }
}