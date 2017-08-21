using Esfa.Vacancy.Register.Application.Queries.GetVacancy;
using MediatR;
using System.Threading.Tasks;
using AutoMapper;

namespace Esfa.Vacancy.Register.Api.Orchestrators
{
    public class VacancyOrchestrator
    {
        private readonly IMediator _mediator;

        public VacancyOrchestrator(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<Vacancy.Api.Types.Vacancy> GetVacancyDetailsAsync(int id)
        {
            var response = await _mediator.Send(new GetVacancyRequest() { Reference = id });

            return response == null ? null : Mapper.Map<Vacancy.Api.Types.Vacancy>(response.Vacancy);
        }
    }
}
