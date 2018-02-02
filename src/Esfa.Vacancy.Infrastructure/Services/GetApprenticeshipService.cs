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
    public class GetApprenticeshipService : IGetApprenticeshipService
    {
        private readonly SqlDatabaseService _sqlDatabaseService;
        private readonly ILog _logger;
        private const string GetLiveApprenticeshipVacancyByReferenceNumberSqlSproc = "[VACANCY_API].[GetLiveApprenticeshipVacancy]";

        public GetApprenticeshipService(SqlDatabaseService sqlDatabaseService, ILog logger)
        {
            _sqlDatabaseService = sqlDatabaseService;
            _logger = logger;
        }

        public async Task<ApprenticeshipVacancy> GetApprenticeshipVacancyByReferenceNumberAsync(int referenceNumber)
        {
            return await _sqlDatabaseService.ExecuteWithRetryAsync<ApprenticeshipVacancy>(
                "Get Apprenticeship Vacancy Details", async sqlConn =>
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@ReferenceNumber", referenceNumber, DbType.Int32);

                    _logger.Info(
                        $"Querying AVMS database to get Apprenticeship Vacancy details for {referenceNumber}.");

                    var results =
                        await sqlConn.QueryAsync<ApprenticeshipVacancy, Address, ApprenticeshipVacancy>(
                            GetLiveApprenticeshipVacancyByReferenceNumberSqlSproc,
                            param: parameters,
                            map: (v, a) =>
                            {
                                v.Location = a;
                                return v;
                            },
                            splitOn: "AddressLine1",
                            commandType: CommandType.StoredProcedure);

                    var apprenticeshipVacancy = results.FirstOrDefault();

                    if (apprenticeshipVacancy != null)
                        _logger.Info(
                            $"Retrieved Apprenticeship Vacancy details for {referenceNumber} from AVMS database.");


                    return apprenticeshipVacancy;
                });
        }
    }
}