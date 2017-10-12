using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Esfa.Vacancy.Register.Domain.Repositories;
using Esfa.Vacancy.Register.Infrastructure.Settings;
using SFA.DAS.NLog.Logger;
using DomainEntities = Esfa.Vacancy.Register.Domain.Entities;

namespace Esfa.Vacancy.Register.Infrastructure.Repositories
{
    public class VacancyRepository : IVacancyRepository
    {
        private const string GetLiveVacancyByReferenceNumberSqlSproc = "[VACANCY_API].[GetLiveVacancy]";
        private readonly IProvideSettings _provideSettings;
        private readonly ILog _logger;

        public VacancyRepository(IProvideSettings provideSettings, ILog logger)
        {
            _provideSettings = provideSettings;
            _logger = logger;
        }

        public async Task<DomainEntities.Vacancy> GetVacancyByReferenceNumberAsync(int referenceNumber)
        {
            var retry = VacancyRegisterRetryPolicy.GetFixedIntervalPolicy((exception, time, retryCount, context) =>
            {
                _logger.Warn($"Error retrieving vacancy from database: ({exception.Message}). Retrying...attempt {retryCount})");
            });
            
            return await retry.ExecuteAsync(() => InternalGetVacancyByReferenceNumberAsync(referenceNumber));
        }

        private async Task<DomainEntities.Vacancy> InternalGetVacancyByReferenceNumberAsync(int referenceNumber)
        {
            var connectionString =
                _provideSettings.GetSetting(ApplicationSettingKeyConstants.AvmsPlusDatabaseConnectionStringKey);

            DomainEntities.Vacancy vacancy;

            using (var sqlConn = new SqlConnection(connectionString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@ReferenceNumber", referenceNumber, DbType.Int32);

                await sqlConn.OpenAsync();
                var results =
                    await sqlConn.QueryAsync<DomainEntities.Vacancy, DomainEntities.Address, DomainEntities.Vacancy>(
                        GetLiveVacancyByReferenceNumberSqlSproc,
                        param: parameters,
                        map: (v, a) => { v.Location = a; return v; },
                        splitOn: "AddressLine1",
                        commandType: CommandType.StoredProcedure);

                vacancy = results.FirstOrDefault();
            }

            return vacancy;
        }
    }
}