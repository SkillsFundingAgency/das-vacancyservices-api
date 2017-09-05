using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Esfa.Vacancy.Register.Domain.Repositories;
using Esfa.Vacancy.Register.Infrastructure.Settings;
using DomainEntities = Esfa.Vacancy.Register.Domain.Entities;

namespace Esfa.Vacancy.Register.Infrastructure.Repositories
{
    public class VacancyRepository : IVacancyRepository
    {
        private const string GetLiveVacancyByReferenceNumberSqlQuery = "EXEC [VACANCY_API].[GetLiveVacancy] @ReferenceNumber";
        private readonly IProvideSettings _provideSettings;

        public VacancyRepository(IProvideSettings provideSettings)
        {
            _provideSettings = provideSettings;
        }

        public async Task<DomainEntities.Vacancy> GetVacancyByReferenceNumberAsync(int referenceNumber)
        {
            var connectionString =
                _provideSettings.GetSetting(ApplicationSettingConstants.AvmsPlusDatabaseConnectionStringKey);

            DomainEntities.Vacancy vacancy;

            using (var sqlConn = new SqlConnection(connectionString))
            {
                await sqlConn.OpenAsync();
                var results =
                    await sqlConn.QueryAsync<DomainEntities.Vacancy, DomainEntities.Address, DomainEntities.Vacancy>(
                        GetLiveVacancyByReferenceNumberSqlQuery,
                        param: new { ReferenceNumber = referenceNumber },
                        map: (v, a) => { v.Location = a; return v; },
                        splitOn: "AddressLine1");

                vacancy = results.FirstOrDefault();
            }

            return vacancy;
        }
    }
}