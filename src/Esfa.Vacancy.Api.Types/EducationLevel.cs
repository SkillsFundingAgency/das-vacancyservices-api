using System.ComponentModel;

namespace Esfa.Vacancy.Api.Types
{
    public enum EducationLevel
    {
        Intermediate = 1,
        Advanced = 2,
        Higher = 3,
        [Description("Foundation degree")]
        Foundation = 5,
        Degree = 6,
        [Description("Master's degree")]
        Masters = 7
    }
}
