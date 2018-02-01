using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Esfa.Vacancy.Domain.Entities;
using Esfa.Vacancy.Domain.Interfaces;
using Esfa.Vacancy.Infrastructure.Factories;

namespace Esfa.Vacancy.Infrastructure.Services
{
    public class VacancyOwnerService : IVacancyOwnerService
    {
        private readonly SqlDatabaseService _sqlDatabaseService;
        private const string GetVacancyOwnerRelationshipStoredProc = "[VACANCY_API].[GetEmployersInformation]";

        public VacancyOwnerService(SqlDatabaseService sqlDatabaseService)
        {
            _sqlDatabaseService = sqlDatabaseService;
        }

        public async Task<EmployerInformation> GetEmployersInformationAsync(int providerUkprn,
            int providerSiteEdsUrn, int employerEdsUrn)
        {
            var result = await _sqlDatabaseService.ExecuteWithRetryAsync(
                "Get provider site and employer's link",
                async (sqlConn) =>
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("ProviderUkprn", providerUkprn, DbType.Int32);
                    parameters.Add("ProviderSiteEdsUrn", providerSiteEdsUrn, DbType.Int32);
                    parameters.Add("EmployerEdsUrn", employerEdsUrn, DbType.Int32);

                    var results = await sqlConn.QueryAsync<EmployerInformation>(
                        GetVacancyOwnerRelationshipStoredProc, parameters,
                        commandType: CommandType.StoredProcedure);

                    return results.FirstOrDefault();
                });
            return result;
        }
    }
}