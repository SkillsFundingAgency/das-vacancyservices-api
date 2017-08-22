using System;

namespace Esfa.Vacancy.Register.Domain.Entities
{
    public sealed class Vacancy
    {
        public int Reference { get; set; }

        public string Title { get; set; }

        public string ShortDescription { get; set; }

        public string Description { get; set; }

        public int? VacancyTypeId { get; set; }

        public int? WageUnitId { get; set; }

        public decimal? WeeklyWage { get; set; }

        public string WorkingWeek { get; set; }

        public string WageText { get; set; }

        public decimal? HoursPerWeek { get; set; }

        public string ExpectedDuration { get; set; }

        public DateTime? ExpectedStartDate { get; set; }

        public DateTime? DatePosted { get; set; }

        public DateTime? ApplicationClosingDate { get; set; }

        public DateTime? InterviewFromDate { get; set; }

        public int NumberOfPositions { get; set; }

        public int? StandardCode { get; set; }

        public string StandardTitle { get; set; }

        public int? FrameworkCode { get; set; }

        public string FrameworkTitle { get; set; }

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

        public Address EmployerAddress { get; set; }

        public int VacancyStatusId { get; set; }

        public bool IsAnonymousEmployer => string.IsNullOrEmpty(AnonymousEmployerName) == false;

        public string ContractOwnerName { get; set; }
    }
}
