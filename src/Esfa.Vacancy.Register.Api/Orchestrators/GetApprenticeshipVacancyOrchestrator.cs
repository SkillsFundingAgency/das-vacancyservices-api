using System.Threading.Tasks;
using Esfa.Vacancy.Register.Api.Mappings;
using Esfa.Vacancy.Register.Application.Queries.GetApprenticeshipVacancy;
using FluentValidation;
using MediatR;

namespace Esfa.Vacancy.Register.Api.Orchestrators
{
    public class GetApprenticeshipVacancyOrchestrator
    {
        private readonly IMediator _mediator;
        private readonly IApprenticeshipMapper _mapper;

        public GetApprenticeshipVacancyOrchestrator(IMediator mediator, IApprenticeshipMapper apprenticeshipMapper)
        {
            _mediator = mediator;
            _mapper = apprenticeshipMapper;
        }

        public async Task<Vacancy.Api.Types.ApprenticeshipVacancy> GetApprenticeshipVacancyDetailsAsync(string id)
        {
            int parsedId;
            if (!int.TryParse(id, out parsedId))
            {
                throw new ValidationException("todo");
            }

            var response = await _mediator.Send(new GetApprenticeshipVacancyRequest() { Reference = parsedId });
            var vacancy = _mapper.MapToApprenticeshipVacancy(response.ApprenticeshipVacancy);

            return vacancy;
        }
    }
}
