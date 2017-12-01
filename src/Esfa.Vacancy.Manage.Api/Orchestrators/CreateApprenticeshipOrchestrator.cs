using System.Threading.Tasks;
using Esfa.Vacancy.Api.Types;
using Esfa.Vacancy.Application.Commands.CreateApprenticeship;
using Esfa.Vacancy.Manage.Api.Mappings;
using MediatR;

namespace Esfa.Vacancy.Manage.Api.Orchestrators
{
    public class CreateApprenticeshipOrchestrator
    {
        private readonly IMediator _mediator;
        private readonly ICreateApprenticeshipResponseMapper _apprenticeshipResponseMapper;

        public CreateApprenticeshipOrchestrator(IMediator mediator, ICreateApprenticeshipResponseMapper apprenticeshipResponseMapper)
        {
            _mediator = mediator;
            _apprenticeshipResponseMapper = apprenticeshipResponseMapper;
        }

        public async Task<Vacancy.Api.Types.CreateApprenticeshipResponse> CreateApprecticeship(CreateApprenticeshipParameters parameters)
        {
            var response = await _mediator.Send(new CreateApprenticeshipRequest());
            return _apprenticeshipResponseMapper.MapToApiResponse(response);
        }
    }
}
