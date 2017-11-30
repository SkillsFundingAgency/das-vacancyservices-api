using Esfa.Vacancy.Infrastructure.Settings;
using TraineeshipVacancy = Esfa.Vacancy.Api.Types.TraineeshipVacancy;

namespace Esfa.Vacancy.Register.Api.Mappings
{
    public sealed class TraineeshipMapper : ITraineeshipMapper
    {
        private readonly IProvideSettings _provideSettings;
        private readonly AddressMapper _addressMapper = new AddressMapper();
        private const int Nationwide = 3;

        public TraineeshipMapper(IProvideSettings provideSettings)
        {
            _provideSettings = provideSettings;
        }

        public TraineeshipVacancy MapToTraineeshipVacancy(Domain.Entities.TraineeshipVacancy traineeshipVacancy)
        {
            var liveVacancyBaseUrl = _provideSettings.GetSetting(ApplicationSettingKeyConstants.LiveTraineeshipVacancyBaseUrlKey);

            return new TraineeshipVacancy
            {
                VacancyReference = traineeshipVacancy.VacancyReferenceNumber,
                Title = traineeshipVacancy.Title,
                ShortDescription = traineeshipVacancy.ShortDescription,
                Description = traineeshipVacancy.Description,
                WorkingWeek = traineeshipVacancy.WorkingWeek,
                ExpectedDuration = traineeshipVacancy.ExpectedDuration,
                ExpectedStartDate = traineeshipVacancy.ExpectedStartDate,
                PostedDate = traineeshipVacancy.PostedDate,
                ApplicationClosingDate = traineeshipVacancy.ApplicationClosingDate,
                InterviewFromDate = traineeshipVacancy.InterviewFromDate,
                NumberOfPositions = traineeshipVacancy.NumberOfPositions,
                TraineeshipSector = traineeshipVacancy.TraineeshipSector,
                EmployerName = traineeshipVacancy.IsAnonymousEmployer ? traineeshipVacancy.AnonymousEmployerName : traineeshipVacancy.EmployerName,
                EmployerDescription = traineeshipVacancy.IsAnonymousEmployer ? traineeshipVacancy.AnonymousEmployerDescription : traineeshipVacancy.EmployerDescription,
                EmployerWebsite = traineeshipVacancy.IsAnonymousEmployer ? null : traineeshipVacancy.EmployerWebsite,
                TrainingToBeProvided = traineeshipVacancy.TrainingToBeProvided,
                QualificationsRequired = traineeshipVacancy.QualificationsRequired,
                SkillsRequired = traineeshipVacancy.SkillsRequired,
                PersonalQualities = traineeshipVacancy.PersonalQualities,
                ImportantInformation = traineeshipVacancy.ImportantInformation,
                FutureProspects = traineeshipVacancy.FutureProspects,
                ThingsToConsider = traineeshipVacancy.ThingsToConsider,
                IsNationwide = traineeshipVacancy.VacancyLocationTypeId == Nationwide,
                SupplementaryQuestion1 = traineeshipVacancy.SupplementaryQuestion1,
                SupplementaryQuestion2 = traineeshipVacancy.SupplementaryQuestion2,
                VacancyUrl = $"{liveVacancyBaseUrl}/{traineeshipVacancy.VacancyReferenceNumber}",
                Location = _addressMapper.MapToLocation(traineeshipVacancy.Location, showAnonymousEmployerDetails: traineeshipVacancy.IsAnonymousEmployer),
                ContactName = traineeshipVacancy.ContactName,
                ContactEmail = traineeshipVacancy.ContactEmail,
                ContactNumber = traineeshipVacancy.ContactNumber,
                TrainingProviderName = traineeshipVacancy.TrainingProvider,
                TrainingProviderUkprn = traineeshipVacancy.TrainingProviderUkprn,
                TrainingProviderSite = traineeshipVacancy.TrainingProviderSite,
                IsEmployerDisabilityConfident = traineeshipVacancy.IsDisabilityConfident
            };
        }
    }
}