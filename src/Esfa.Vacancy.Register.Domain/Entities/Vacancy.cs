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

        public DateTime ExpectedStartDate { get; set; }

        public DateTime DatePosted { get; set; }

        public DateTime ApplicationClosingDate { get; set; }

        public int NumberOfPositions { get; set; }

        public int? StandardCode { get; set; }

        public string FrameworkCode { get; set; }

        public string EmployerName { get; set; }

        public string EmployerDescription { get; set; }

        public string EmployerWebsite { get; set; }

        public string TrainingToBeProvided { get; set; }

        public string QulificatiosRequired { get; set; }

        public string SkillsRequired { get; set; }

        public string PersonalQualities { get; set; }

        public string ImportantInformation { get; set; }

        public string FutureProspects { get; set; }

        public string RealityCheck { get; set; }

        public Address EmployerAddress { get; set; }
    }
}
