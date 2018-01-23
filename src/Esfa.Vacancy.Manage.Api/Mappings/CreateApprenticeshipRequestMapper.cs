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
                LocationType = (ApplicationTypes.LocationType)(int)parameters.LocationType,
                AddressLine1 = parameters.Location.AddressLine1,
                AddressLine2 = parameters.Location.AddressLine2,
                AddressLine3 = parameters.Location.AddressLine3,
                AddressLine4 = parameters.Location.AddressLine4,
                AddressLine5 = parameters.Location.AddressLine5,
                Town = parameters.Location.Town,
                Postcode = parameters.Location.Postcode,
                NumberOfPositions = parameters.NumberOfPositions
            };
        }
    }
}