using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Esfa.Vacancy.Domain.Entities;
using Esfa.Vacancy.Domain.Interfaces;
using Esfa.Vacancy.Infrastructure.Factories;
using SFA.DAS.NLog.Logger;

namespace Esfa.Vacancy.Infrastructure.Services
{
    public class GetTraineeshipService : IGetTraineeshipService
    {
        private const string GetLiveTraineeshipVacancyByReferenceNumberSqlSproc = "[VACANCY_API].[GetLiveTraineeshipVacancy]";

        private readonly SqlDatabaseService _sqlDatabaseService;
        private readonly ILog _logger;

        public GetTraineeshipService(SqlDatabaseService sqlDatabaseService, ILog logger)
        {
            _sqlDatabaseService = sqlDatabaseService;
            _logger = logger;
        }
        public async Task<TraineeshipVacancy> GetTraineeshipVacancyByReferenceNumberAsync(int referenceNumber)
        {
            return await _sqlDatabaseService.ExecuteWithRetryAsync<TraineeshipVacancy>(
                "Get Traineeship Vacancy Details", async (sqlConn) =>
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@ReferenceNumber", referenceNumber, DbType.Int32);

                    _logger.Info($"Querying AVMS database to get Traineeship Vacancy details for {referenceNumber}.");

                    var results =
                        await sqlConn.QueryAsync<TraineeshipVacancy, Address, TraineeshipVacancy>(
                            GetLiveTraineeshipVacancyByReferenceNumberSqlSproc,
                            param: parameters,
                            map: (v, a) =>
                            {
                                v.Location = a;
                                return v;
                            },
                            splitOn: "AddressLine1",
                            commandType: CommandType.StoredProcedure);

                    var traineeshipVacancy = results.FirstOrDefault();

                    if (traineeshipVacancy != null)
                        _logger.Info($"Retrieved Traineeship Vacancy details for {referenceNumber} from AVMS database.");

                    return traineeshipVacancy;
                });
        }
    }
}