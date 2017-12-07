using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Esfa.Vacancy.Domain.Entities;
using Esfa.Vacancy.Domain.Repositories;
using Esfa.Vacancy.Infrastructure.Settings;
using SFA.DAS.NLog.Logger;

namespace Esfa.Vacancy.Infrastructure.Repositories
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

        public async Task<ApprenticeshipVacancy> GetApprenticeshipVacancyByReferenceNumberAsync(int referenceNumber)
        {
            var retry = PollyRetryPolicies.GetFixedIntervalPolicy((exception, time, retryCount, context) =>
            {
                _logger.Warn($"Error retrieving apprenticeship vacancy from database: ({exception.Message}). Retrying... attempt {retryCount}");
            });

            return await retry.ExecuteAsync(() => InternalGetVacancyByReferenceNumberAsync(referenceNumber));
        }

        private async Task<ApprenticeshipVacancy> InternalGetVacancyByReferenceNumberAsync(int referenceNumber)
        {
            var connectionString =
                _provideSettings.GetSetting(ApplicationSettingKeyConstants.AvmsPlusDatabaseConnectionStringKey);

            ApprenticeshipVacancy apprenticeshipVacancy;

            using (var sqlConn = new SqlConnection(connectionString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@ReferenceNumber", referenceNumber, DbType.Int32);

                _logger.Info($"Querying AVMS database to get Apprenticeship Vacancy details for {referenceNumber}.");

                await sqlConn.OpenAsync();
                var results =
                    await sqlConn.QueryAsync<ApprenticeshipVacancy, Address, ApprenticeshipVacancy>(
                        GetLiveApprenticeshipVacancyByReferenceNumberSqlSproc,
                        param: parameters,
                        map: (v, a) => { v.Location = a; return v; },
                        splitOn: "AddressLine1",
                        commandType: CommandType.StoredProcedure);

                apprenticeshipVacancy = results.FirstOrDefault();

                if (apprenticeshipVacancy != null)
                    _logger.Info($"Retrieved Apprenticeship Vacancy details for {referenceNumber} from AVMS database.");
            }

            return apprenticeshipVacancy;
        }

        public async Task<TraineeshipVacancy> GetTraineeshipVacancyByReferenceNumberAsync(int referenceNumber)
        {
            var connectionString =
                _provideSettings.GetSetting(ApplicationSettingKeyConstants.AvmsPlusDatabaseConnectionStringKey);

            TraineeshipVacancy traineeshipVacancy;

            using (var sqlConn = new SqlConnection(connectionString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@ReferenceNumber", referenceNumber, DbType.Int32);

                _logger.Info($"Querying AVMS database to get Traineeship Vacancy details for {referenceNumber}.");

                await sqlConn.OpenAsync();
                var results =
                    await sqlConn.QueryAsync<TraineeshipVacancy, Address, TraineeshipVacancy>(
                        GetLiveTraineeshipVacancyByReferenceNumberSqlSproc,
                        param: parameters,
                        map: (v, a) => { v.Location = a; return v; },
                        splitOn: "AddressLine1",
                        commandType: CommandType.StoredProcedure);

                traineeshipVacancy = results.FirstOrDefault();

                if (traineeshipVacancy != null)
                    _logger.Info($"Retrieved Traineeship Vacancy details for {referenceNumber} from AVMS database.");
            }

            return traineeshipVacancy;
        }

        public Task<int> CreateApprenticeshipAsync(CreateApprenticeshipParameters parameters)
        {
            throw new System.NotImplementedException();
        }
    }
}