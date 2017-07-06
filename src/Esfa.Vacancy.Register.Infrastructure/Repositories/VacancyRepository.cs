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
                    await sqlConn.QueryAsync<DomainEntities.Vacancy>(
                        GetVacancyDetailsQuery, new { ReferenceNumber = referenceNumber });
                vacancy = results.FirstOrDefault();
            }

            return vacancy;
        }

        const string GetVacancyDetailsQuery = @"
SELECT  V.VacancyReferenceNumber AS Reference
,       V.Title
,       V.ShortDescription
,       V.[Description]
,       V.VacancyTypeId
,       V.WageUnitId
,       V.WeeklyWage
,       V.WorkingWeek
,       V.WageText
,       V.HoursPerWeek
,       V.ExpectedDuration 
,       V.ExpectedStartDate
,		VH.HistoryDate AS DatePosted
,       V.ApplicationClosingDate
,       V.NumberofPositions 
,       V.EmployerDescription
,       V.EmployersWebsite
,       V.TrainingTypeId
,       RS.LarsCode AS StandardCode
,       AF.ShortName AS FrameworkCode
FROM[dbo].[Vacancy]        V
INNER JOIN (SELECT VacancyId, Min(HistoryDate) HistoryDate
            FROM [dbo].[VacancyHistory]
            WHERE VacancyHistoryEventTypeId = 1
            AND VacancyHistoryEventSubTypeId = 2
            GROUP BY VacancyId
           ) VH
	ON V.VacancyId = VH.VacancyId 
LEFT JOIN   [Reference].[Standard] RS 
    ON V.StandardId = RS.StandardId
LEFT JOIN   [dbo].[ApprenticeshipFramework] AF
    ON      V.ApprenticeshipFrameworkId = AF.ApprenticeshipFrameworkId
WHERE V.VacancyStatusId = 2
AND V.VacancyReferenceNumber = @ReferenceNumber";
    }
}