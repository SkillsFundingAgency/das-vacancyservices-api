using Esfa.Vacancy.Api.Types.Request;
using Esfa.Vacancy.Application.Commands.CreateApprenticeship;

namespace Esfa.Vacancy.Manage.Api.Mappings
{
    public interface ICreateApprenticeshipRequestMapper
    {
        CreateApprenticeshipRequest MapFromApiParameters(
            CreateApprenticeshipParameters parameters,
            int providerUkprn);
    }
}