using Esfa.Vacancy.Api.Types;

namespace Esfa.Vacancy.Register.Api.Mappings
{
    public interface ITraineeshipMapper
    {
        TraineeshipVacancy MapToTraineeshipVacancy(Domain.Entities.TraineeshipVacancy traineeshipVacancy);
    }
}