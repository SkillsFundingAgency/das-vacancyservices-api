using System.Threading.Tasks;
using Esfa.Vacancy.Api.Types;
using Esfa.Vacancy.Register.Api.Mappings;
using Esfa.Vacancy.Register.Application.Queries.GetTraineeshipVacancy;
using MediatR;

namespace Esfa.Vacancy.Register.Api.Orchestrators
{
    public class GetTraineeshipVacancyOrchestrator
    {
        private readonly IMediator _mediator;
        private readonly ITraineeshipMapper _mapper;

        public GetTraineeshipVacancyOrchestrator(IMediator mediator, ITraineeshipMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        public async Task<TraineeshipVacancy> GetTraineeshipVacancyDetailsAsync(int id)
        {
            var response = await _mediator.Send(new GetTraineeshipVacancyRequest() { Reference = id });
            var vacancy = _mapper.MapToTraineeshipVacancy(response.TraineeshipVacancy);
            return vacancy;
        }
    }
}
