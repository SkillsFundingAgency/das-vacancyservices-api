using System.Threading.Tasks;
using Esfa.Vacancy.Domain.Entities;

namespace Esfa.Vacancy.Domain.Interfaces
{
    public interface IVacancyOwnerService
    {
        Task<EmployerInformation> GetEmployersInformationAsync(int providerUkprn,
            int providerSiteEdsUrn, int employerEdsUrn);
    }
}