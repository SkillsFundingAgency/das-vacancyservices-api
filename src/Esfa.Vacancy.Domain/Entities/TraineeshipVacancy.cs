using System;

namespace Esfa.Vacancy.Domain.Entities
{
    public sealed class TraineeshipVacancy
    {
        public int VacancyReferenceNumber { get; set; }

        public string Title { get; set; }

        public string ShortDescription { get; set; }

        public string Description { get; set; }

        public int? VacancyTypeId { get; set; }

        public string WorkingWeek { get; set; }

        public string ExpectedDuration { get; set; }

        public DateTime? ExpectedStartDate { get; set; }

        public DateTime PostedDate { get; set; }

        public DateTime? ApplicationClosingDate { get; set; }

        public DateTime? InterviewFromDate { get; set; }

        public int NumberOfPositions { get; set; }

        public string TraineeshipSector { get; set; }

        public string EmployerName { get; set; }

        public string EmployerDescription { get; set; }

        public string AnonymousEmployerName { get; set; }

        public string AnonymousEmployerDescription { get; set; }

        public string AnonymousEmployerReason { get; set; }

        public string EmployerWebsite { get; set; }

        public string TrainingToBeProvided { get; set; }

        public string QualificationsRequired { get; set; }

        public string SkillsRequired { get; set; }

        public string PersonalQualities { get; set; }

        public string ImportantInformation { get; set; }

        public string FutureProspects { get; set; }

        public string ThingsToConsider { get; set; }

        public int VacancyLocationTypeId { get; set; }

        public string SupplementaryQuestion1 { get; set; }

        public string SupplementaryQuestion2 { get; set; }

        public Address Location { get; set; }

        public int VacancyStatusId { get; set; }

        public bool IsAnonymousEmployer => string.IsNullOrEmpty(AnonymousEmployerName) == false;

        public string ContactName { get; set; }

        public string ContactEmail { get; set; }

        public string ContactNumber { get; set; }

        public string TrainingProvider { get; set; }

        public string TrainingProviderUkprn { get; set; }

        public string TrainingProviderSite { get; set; }

        public bool IsDisabilityConfident { get; set; }

        public string EmployersRecruitmentWebsite { get; set; }

        public string EmployersApplicationInstructions { get; set; }
    }
}
