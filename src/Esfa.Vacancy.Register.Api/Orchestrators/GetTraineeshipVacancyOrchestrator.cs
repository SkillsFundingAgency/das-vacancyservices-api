using System.Threading.Tasks;
using Esfa.Vacancy.Api.Types;
using Esfa.Vacancy.Register.Application.Queries.GetVacancy;
using Esfa.Vacancy.Register.Infrastructure.Settings;
using MediatR;

namespace Esfa.Vacancy.Register.Api.Orchestrators
{
    public class GetTraineeshipVacancyOrchestrator
    {
        private readonly IMediator _mediator;
        private readonly IProvideSettings _provideSettings;

        public GetTraineeshipVacancyOrchestrator(IMediator mediator, IProvideSettings provideSettings)
        {
            _mediator = mediator;
            _provideSettings = provideSettings;
        }

        public async Task<TraineeshipVacancy> GetTraineeshipVacancyDetailsAsync(int id)
        {
            var liveVacancyBaseUrl = _provideSettings.GetSetting(ApplicationSettingKeyConstants.LiveVacancyBaseUrlKey);
            var response = await _mediator.Send(new GetTraineeshipVacancyRequest() { Reference = id });
            var vacancy = MapToTraineeship(response.Vacancy);
            vacancy.VacancyUrl = $"{liveVacancyBaseUrl}/{vacancy.VacancyReference}";
            return vacancy;
        }

        private TraineeshipVacancy MapToTraineeship(Domain.Entities.Vacancy vacancy)
        {
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
                EmployerName = vacancy.EmployerName,
                EmployerDescription = vacancy.EmployerDescription,
                EmployerWebsite = vacancy.EmployerWebsite,
                TrainingToBeProvided = vacancy.TrainingToBeProvided,
                QualificationsRequired = vacancy.QualificationsRequired,
                SkillsRequired = vacancy.SkillsRequired,
                PersonalQualities = vacancy.PersonalQualities,
                ImportantInformation = vacancy.ImportantInformation,
                FutureProspects = vacancy.FutureProspects,
                ThingsToConsider = vacancy.ThingsToConsider,
                LocationType = (VacancyLocationType) vacancy.VacancyLocationTypeId,
                SupplementaryQuestion1 = vacancy.SupplementaryQuestion1,
                SupplementaryQuestion2 = vacancy.SupplementaryQuestion2,
                Location = MapToLocation(vacancy.Location),
                ContactName = vacancy.ContactName,
                ContactEmail = vacancy.ContactEmail,
                ContactNumber = vacancy.ContactNumber
            };
        }

        private Address MapToLocation(Domain.Entities.Address location)
        {
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
