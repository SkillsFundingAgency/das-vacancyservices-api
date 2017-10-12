using Esfa.Vacancy.Api.Types;
using Esfa.Vacancy.Register.Infrastructure.Settings;

namespace Esfa.Vacancy.Register.Api.Mappings
{
    public sealed class VacancyMapper
    {
        private readonly IProvideSettings _provideSettings;

        public VacancyMapper(IProvideSettings provideSettings)
        {
            _provideSettings = provideSettings;
        }

        public TraineeshipVacancy MapToTraineeshipVacancy(Domain.Entities.Vacancy vacancy)
        {
            var liveVacancyBaseUrl = _provideSettings.GetSetting(ApplicationSettingKeyConstants.LiveTraineeshipVacancyBaseUrlKey);

            return new TraineeshipVacancy
            {
                VacancyReference = vacancy.VacancyReferenceNumber,
                Title = vacancy.Title,
                ShortDescription = vacancy.ShortDescription,
                Description = vacancy.Description,
                WorkingWeek = vacancy.WorkingWeek,
                ExpectedDuration = vacancy.ExpectedDuration,
                ExpectedStartDate = vacancy.ExpectedStartDate,
                PostedDate = vacancy.PostedDate,
                ApplicationClosingDate = vacancy.ApplicationClosingDate,
                InterviewFromDate = vacancy.InterviewFromDate,
                NumberOfPositions = vacancy.NumberOfPositions,
                EmployerName = vacancy.IsAnonymousEmployer ? vacancy.AnonymousEmployerName : vacancy.EmployerName,
                EmployerDescription = vacancy.IsAnonymousEmployer ? vacancy.AnonymousEmployerDescription : vacancy.EmployerDescription,
                EmployerWebsite = vacancy.IsAnonymousEmployer ? null : vacancy.EmployerWebsite,
                TrainingToBeProvided = vacancy.TrainingToBeProvided,
                QualificationsRequired = vacancy.QualificationsRequired,
                SkillsRequired = vacancy.SkillsRequired,
                PersonalQualities = vacancy.PersonalQualities,
                ImportantInformation = vacancy.ImportantInformation,
                FutureProspects = vacancy.FutureProspects,
                ThingsToConsider = vacancy.ThingsToConsider,
                LocationType = (VacancyLocationType)vacancy.VacancyLocationTypeId,
                SupplementaryQuestion1 = vacancy.SupplementaryQuestion1,
                SupplementaryQuestion2 = vacancy.SupplementaryQuestion2,
                VacancyUrl = $"{liveVacancyBaseUrl}/{vacancy.VacancyReferenceNumber}",
                Location = MapToLocation(vacancy.Location, showAnonymousEmployerDetails: vacancy.IsAnonymousEmployer),
                ContactName = vacancy.ContactName,
                ContactEmail = vacancy.ContactEmail,
                ContactNumber = vacancy.ContactNumber
            };
        }

        private Address MapToLocation(Domain.Entities.Address location, bool showAnonymousEmployerDetails)
        {
            if (showAnonymousEmployerDetails)
            {
                return new Address
                {
                    Town = location.Town
                };
            }

            return new Address
            {
                AddressLine1 = location.AddressLine1,
                AddressLine2 = location.AddressLine2,
                AddressLine3 = location.AddressLine3,
                AddressLine4 = location.AddressLine4,
                AddressLine5 = location.AddressLine5,
                Latitude = location.Latitude,
                Longitude = location.Longitude,
                PostCode = location.PostCode,
                Town = location.Town
            };
        }
    }
}