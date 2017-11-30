namespace Esfa.Vacancy.Domain.Entities
{
    public enum VacancyStatus
    {
        Unknown = 0,
        Draft = 1,
        Live = 2,
        Referred = 3,
        Deleted = 4,
        Submitted = 5,
        Closed = 6,
        Withdrawn = 7,
        Completed = 8,
        PostedInError = 9,
        ReservedForQA = 10
    }
}
