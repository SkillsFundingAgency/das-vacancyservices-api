using System.Collections.Generic;
using Esfa.Vacancy.Application.Commands.CreateApprenticeship;

namespace Esfa.Vacancy.Manage.Api.Mappings
{
    public interface ICreateApprenticeshipRequestMapper
    {
        CreateApprenticeshipRequest MapFromApiParameters(
            Vacancy.Api.Types.CreateApprenticeshipParameters parameters,
            Dictionary<string, string> headers);
    }
}