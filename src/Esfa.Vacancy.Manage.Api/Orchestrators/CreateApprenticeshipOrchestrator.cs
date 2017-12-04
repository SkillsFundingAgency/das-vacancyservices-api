using System.Threading.Tasks;
using Esfa.Vacancy.Api.Types;
using Esfa.Vacancy.Manage.Api.Mappings;
using MediatR;

namespace Esfa.Vacancy.Manage.Api.Orchestrators
{
    public class CreateApprenticeshipOrchestrator
    {
        private readonly ICreateApprenticeshipRequestMapper _createApprenticeshipRequestMapper;
        private readonly IMediator _mediator;
        private readonly ICreateApprenticeshipResponseMapper _apprenticeshipResponseMapper;

        public CreateApprenticeshipOrchestrator(
            ICreateApprenticeshipRequestMapper createApprenticeshipRequestMapper,
            IMediator mediator, ICreateApprenticeshipResponseMapper apprenticeshipResponseMapper)
        {
            _createApprenticeshipRequestMapper = createApprenticeshipRequestMapper;
            _mediator = mediator;
            _apprenticeshipResponseMapper = apprenticeshipResponseMapper;
        }

        public async Task<CreateApprenticeshipResponse> CreateApprecticeshipAsync(CreateApprenticeshipParameters parameters)
        {
            var request = _createApprenticeshipRequestMapper.MapFromApiParameters(parameters);
            var response = await _mediator.Send(request);
            return _apprenticeshipResponseMapper.MapToApiResponse(response);
        }
    }
}
