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
                _provideSettings.GetSetting(ApplicationSettingKeyConstants.AvmsPlusDatabaseConnectionStringKey);

            DomainEntities.Vacancy vacancy;

            using (var sqlConn = new SqlConnection(connectionString))
            {
                await sqlConn.OpenAsync();
                var results =
                    await sqlConn.QueryAsync<DomainEntities.Vacancy, DomainEntities.Address, DomainEntities.Vacancy>(
                        VacancyDetailsQuery,
                        param: new { ReferenceNumber = referenceNumber },
                        map: (v, a) => { v.Location = a; return v; },
                        splitOn: "AddressLine1");

                vacancy = results.FirstOrDefault();
            }

            return vacancy;
        }

        const string VacancyDetailsQuery = @"
SELECT  V.VacancyReferenceNumber 
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
,		VH.HistoryDate AS PostedDate
,       V.ApplicationClosingDate
,       V.InterviewsFromDate AS InterviewFromDate
,       V.NumberofPositions 
,       V.TrainingTypeId
,       V.VacancyLocationTypeId
,       RS.LarsCode AS StandardCode
,       CAST(AF.CodeName AS INT) AS FrameworkCode
,       E.FullName AS EmployerName
,       V.EmployerDescription AS EmployerDescription
,       V.EmployerAnonymousName AS AnonymousEmployerName
,       V.AnonymousAboutTheEmployer AS AnonymousEmployerDescription
,       V.EmployerAnonymousReason AS AnonymousEmployerReason
,       V.EmployersWebsite
,       TextFields.[TrainingToBeProvided]
,       TextFields.[QulificationsRequired] AS QualificationsRequired
,       TextFields.[SkillsRequired]
,       TextFields.[PersonalQualities]
,       TextFields.[ImportantInformation]
,       TextFields.[FutureProspects]
,       TextFields.[ThingsToConsider]
,       AdditionalQuestions.SupplementaryQuestion1
,       AdditionalQuestions.SupplementaryQuestion2
,       2 AS VacancyStatusId
,       V.ContactName
,       V.ContactEmail
,       V.ContactNumber
,       V.AddressLine1
,       V.AddressLine2
,       V.AddressLine3
,       V.AddressLine4
,       V.AddressLine5
,       V.Latitude
,       V.Longitude
,       V.PostCode
,       V.Town
FROM    [dbo].[Vacancy] V
INNER JOIN (SELECT VacancyId, Min(HistoryDate) HistoryDate
            FROM [dbo].[VacancyHistory]
            WHERE VacancyHistoryEventTypeId = 1
            AND VacancyHistoryEventSubTypeId = 2
            GROUP BY VacancyId
           ) VH
	ON V.VacancyId = VH.VacancyId 
LEFT JOIN   [Reference].[Standard] AS RS 
    ON V.StandardId = RS.StandardId
LEFT JOIN   [dbo].[ApprenticeshipFramework] AS AF
    ON      V.ApprenticeshipFrameworkId = AF.ApprenticeshipFrameworkId
INNER JOIN VacancyOwnerRelationship AS R 
    ON      V.VacancyOwnerRelationshipId = R.VacancyOwnerRelationshipId
INNER JOIN Employer AS E 
    ON      R.EmployerId = E.EmployerId
LEFT JOIN (
            SELECT 
                 VacancyId
            ,    MAX(CASE WHEN Field = 1 THEN [Value] END ) AS [TrainingToBeProvided]
            ,    MAX(CASE WHEN Field = 2 THEN [Value] END ) AS [QulificationsRequired]
            ,    MAX(CASE WHEN Field = 3 THEN [Value] END ) AS [SkillsRequired]
            ,    MAX(CASE WHEN Field = 4 THEN [Value] END ) AS [PersonalQualities]
            ,    MAX(CASE WHEN Field = 5 THEN [Value] END ) AS [ImportantInformation]
            ,    MAX(CASE WHEN Field = 6 THEN [Value] END ) AS [FutureProspects]
            ,    MAX(CASE WHEN Field = 7 THEN [Value] END ) AS [ThingsToConsider]
            FROM VacancyTextField AS T
            GROUP BY VacancyId
          ) AS TextFields
    ON      TextFields.VacancyId = V.VacancyId
LEFT JOIN (
            SELECT 
                 VacancyId
            ,    MAX(CASE WHEN QuestionId = 1 THEN [Question] END ) AS [SupplementaryQuestion1]
            ,    MAX(CASE WHEN QuestionId = 2 THEN [Question] END ) AS [SupplementaryQuestion2]
            FROM AdditionalQuestion AS T
            GROUP BY VacancyId
          ) AS AdditionalQuestions
    ON      AdditionalQuestions.VacancyId = V.VacancyId
WHERE V.VacancyStatusId = 2
AND V.VacancyReferenceNumber = @ReferenceNumber
";
    }
}