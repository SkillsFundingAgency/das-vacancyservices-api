using System.Threading.Tasks;
using Esfa.Vacancy.Register.Api.Mappings;
using Esfa.Vacancy.Register.Application.Queries.GetVacancy;
using Esfa.Vacancy.Register.Infrastructure.Settings;
using MediatR;

namespace Esfa.Vacancy.Register.Api.Orchestrators
{
    public class GetApprenticeshipVacancyOrchestrator
    {
        private readonly IMediator _mediator;
        private VacancyMapper _mapper;

        public GetApprenticeshipVacancyOrchestrator(IMediator mediator, IProvideSettings provideSettings)
        {
            _mediator = mediator;
            _mapper = new VacancyMapper(provideSettings);
        }

        public async Task<Vacancy.Api.Types.ApprenticeshipVacancy> GetApprenticeshipVacancyDetailsAsync(int id)
        {
            var response = await _mediator.Send(new GetApprenticeshipVacancyRequest() { Reference = id });
            var vacancy = _mapper.MapToApprenticeshipVacancy(response.ApprenticeshipVacancy);

            return vacancy;
        }
    }
}
