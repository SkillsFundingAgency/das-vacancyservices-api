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
        private const string GetLiveApprenticeshipVacancyByReferenceNumberSqlSproc = "[VACANCY_API].[GetLiveApprenticeshipVacancy]";
        private const string GetLiveTraineeshipVacancyByReferenceNumberSqlSproc = "[VACANCY_API].[GetLiveTraineeshipVacancy]";
        private readonly IProvideSettings _provideSettings;
        private readonly ILog _logger;

        public VacancyRepository(IProvideSettings provideSettings, ILog logger)
        {
            _provideSettings = provideSettings;
            _logger = logger;
        }

        public async Task<DomainEntities.ApprenticeshipVacancy> GetApprenticeshipVacancyByReferenceNumberAsync(int referenceNumber)
        {
            var retry = VacancyRegisterRetryPolicy.GetFixedIntervalPolicy((exception, time, retryCount, context) =>
            {
                _logger.Warn($"Error retrieving apprenticeship vacancy from database: ({exception.Message}). Retrying... attempt {retryCount}");
            });

            return await retry.ExecuteAsync(() => InternalGetVacancyByReferenceNumberAsync(referenceNumber));
        }

        private async Task<DomainEntities.ApprenticeshipVacancy> InternalGetVacancyByReferenceNumberAsync(int referenceNumber)
        {
            var connectionString =
                _provideSettings.GetSetting(ApplicationSettingKeyConstants.AvmsPlusDatabaseConnectionStringKey);

            DomainEntities.ApprenticeshipVacancy apprenticeshipVacancy;

            using (var sqlConn = new SqlConnection(connectionString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@ReferenceNumber", referenceNumber, DbType.Int32);

                _logger.Info($"Querying AVMS database to get Apprenticeship Vacancy details for {referenceNumber}.");

                await sqlConn.OpenAsync();
                var results =
                    await sqlConn.QueryAsync<DomainEntities.ApprenticeshipVacancy, DomainEntities.Address, DomainEntities.ApprenticeshipVacancy>(
                        GetLiveApprenticeshipVacancyByReferenceNumberSqlSproc,
                        param: parameters,
                        map: (v, a) => { v.Location = a; return v; },
                        splitOn: "AddressLine1",
                        commandType: CommandType.StoredProcedure);

                _logger.Info($"Retrieved Apprenticeship Vacancy details for {referenceNumber} from AVMS database.");

                apprenticeshipVacancy = results.FirstOrDefault();
            }

            return apprenticeshipVacancy;
        }
        public async Task<DomainEntities.TraineeshipVacancy> GetTraineeshipVacancyByReferenceNumberAsync(int referenceNumber)
        {
            var connectionString =
                _provideSettings.GetSetting(ApplicationSettingKeyConstants.AvmsPlusDatabaseConnectionStringKey);

            DomainEntities.TraineeshipVacancy traineeshipVacancy;

            using (var sqlConn = new SqlConnection(connectionString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@ReferenceNumber", referenceNumber, DbType.Int32);

                _logger.Info($"Querying AVMS database to get Traineeship Vacancy details for {referenceNumber}.");

                await sqlConn.OpenAsync();
                var results =
                    await sqlConn.QueryAsync<DomainEntities.TraineeshipVacancy, DomainEntities.Address, DomainEntities.TraineeshipVacancy>(
                        GetLiveTraineeshipVacancyByReferenceNumberSqlSproc,
                        param: parameters,
                        map: (v, a) => { v.Location = a; return v; },
                        splitOn: "AddressLine1",
                        commandType: CommandType.StoredProcedure);

                _logger.Info($"Retrieved Traineeship Vacancy details for {referenceNumber} from AVMS database.");

                traineeshipVacancy = results.FirstOrDefault();
            }

            return traineeshipVacancy;
        }
    }
}