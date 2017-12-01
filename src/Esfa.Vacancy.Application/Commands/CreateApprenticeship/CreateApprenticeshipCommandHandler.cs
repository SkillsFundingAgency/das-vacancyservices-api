using System.Threading.Tasks;
using MediatR;

namespace Esfa.Vacancy.Application.Commands.CreateApprenticeship
{
    public class CreateApprenticeshipCommandHandler : IAsyncRequestHandler<CreateApprenticeshipRequest, CreateApprenticeshipResponse>
    {
        public async Task<CreateApprenticeshipResponse> Handle(CreateApprenticeshipRequest message)
        {
            return await Task.FromResult(new CreateApprenticeshipResponse());
        }
    }
}
