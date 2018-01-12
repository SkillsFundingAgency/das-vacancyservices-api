using Esfa.Vacancy.Api.Types;
using Esfa.Vacancy.Application.Commands.CreateApprenticeship;
using ApplicationTypes = Esfa.Vacancy.Application.Commands.CreateApprenticeship;

namespace Esfa.Vacancy.Manage.Api.Mappings
{
    public class CreateApprenticeshipRequestMapper : ICreateApprenticeshipRequestMapper
    {
        public CreateApprenticeshipRequest MapFromApiParameters(CreateApprenticeshipParameters parameters)
        {
            return new CreateApprenticeshipRequest
            {
                Title = parameters.Title,
                ShortDescription = parameters.ShortDescription,
                LongDescription = parameters.LongDescription,
                ApplicationClosingDate = parameters.ApplicationClosingDate,
                ExpectedStartDate = parameters.ExpectedStartDate,
                WorkingWeek = parameters.WorkingWeek,
                HoursPerWeek = parameters.HoursPerWeek,
                LocationType = (ApplicationTypes.LocationType)(int)parameters.LocationType
            };
        }
    }
}