using Esfa.Vacancy.Application.Commands.CreateApprenticeship;

namespace Esfa.Vacancy.Manage.Api.Mappings
{
    public class CreateApprenticeshipResponseMapper : ICreateApprenticeshipResponseMapper
    {
        public Vacancy.Api.Types.CreateApprenticeshipResponse MapToApiResponse(CreateApprenticeshipResponse createResponse)
        {
            return new Vacancy.Api.Types.CreateApprenticeshipResponse
            {
                VacancyReferenceNumber = createResponse.VacancyReferenceNumber
            };
        }
    }
}