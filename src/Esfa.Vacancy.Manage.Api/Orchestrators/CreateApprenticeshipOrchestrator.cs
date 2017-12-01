using Esfa.Vacancy.Api.Types;
using Esfa.Vacancy.Application.Commands.CreateApprenticeship;
using MediatR;

namespace Esfa.Vacancy.Manage.Api.Orchestrators
{
    public class CreateApprenticeshipOrchestrator
    {
        private readonly IMediator _mediator;

        public CreateApprenticeshipOrchestrator(IMediator mediator)
        {
            _mediator = mediator;
        }

        public CreateApprecticeshipResponse CreateApprecticeship(CreateApprenticeshipParameters parameters)
        {
            _mediator.Send(new CreateApprenticeshipRequest());
            return null;
        }
    }
}
