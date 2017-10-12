using System.Threading.Tasks;
using Esfa.Vacancy.Api.Types;
using Esfa.Vacancy.Register.Api.Mappings;
using Esfa.Vacancy.Register.Application.Queries.GetVacancy;
using Esfa.Vacancy.Register.Infrastructure.Settings;
using MediatR;

namespace Esfa.Vacancy.Register.Api.Orchestrators
{
    public class GetTraineeshipVacancyOrchestrator
    {
        private readonly IMediator _mediator;
        private readonly VacancyMapper _mapper;

        public GetTraineeshipVacancyOrchestrator(IMediator mediator, IProvideSettings provideSettings)
        {
            _mediator = mediator;
            _mapper = new VacancyMapper(provideSettings);
        }

        public async Task<TraineeshipVacancy> GetTraineeshipVacancyDetailsAsync(int id)
        {
            var response = await _mediator.Send(new GetTraineeshipVacancyRequest() { Reference = id });
            var vacancy = _mapper.MapToTraineeshipVacancy(response.Vacancy);
            return vacancy;
        }
    }
}
