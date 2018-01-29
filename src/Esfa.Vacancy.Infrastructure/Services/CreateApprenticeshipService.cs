using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Dapper;
using Esfa.Vacancy.Domain.Entities;
using Esfa.Vacancy.Domain.Interfaces;
using Esfa.Vacancy.Infrastructure.Exceptions;
using Esfa.Vacancy.Infrastructure.Factories;
using Esfa.Vacancy.Infrastructure.Settings;
using SFA.DAS.NLog.Logger;

namespace Esfa.Vacancy.Infrastructure.Services
{
    public class CreateApprenticeshipService : ICreateApprenticeshipService
    {
        private const string CreateApprenticeshipVacancySqlSproc = "[VACANCY_API].[CreateApprenticeshipVacancy]";

        private readonly SqlDatabaseService _sqlDatabaseService;
        private readonly ILog _logger;

        public CreateApprenticeshipService(SqlDatabaseService sqlDatabaseService, ILog logger)
        {
            _sqlDatabaseService = sqlDatabaseService;
            _logger = logger;
        }

        public async Task<int> CreateApprenticeshipAsync(CreateApprenticeshipParameters parameters)
        {
            return await _sqlDatabaseService.ExecuteWithRetryAsync("Create Apprenticeship Vacancy", async sqlConn =>
            {
                var dynamicParameters = new DynamicParameters(parameters);
                dynamicParameters.Add("VacancyReferenceNumber", dbType: DbType.Int32, direction: ParameterDirection.Output);

                _logger.Info($"Command to create new Apprenticeship Vacancy in AVMS database, Title: [{parameters.Title}].");

                await sqlConn.ExecuteAsync(
                    CreateApprenticeshipVacancySqlSproc,
                    dynamicParameters,
                    commandType: CommandType.StoredProcedure);

                var referenceNumber = dynamicParameters.Get<int>("VacancyReferenceNumber");

                if (referenceNumber == int.MinValue)
                    throw new InfrastructureException(new Exception($"Failed to get reference number for new Apprenticeship Vacancy [{parameters.Title}]"));

                _logger.Info($"Created Apprenticeship Vacancy for [{referenceNumber}] from AVMS database.");

                return referenceNumber;

            });
        }

    }
}