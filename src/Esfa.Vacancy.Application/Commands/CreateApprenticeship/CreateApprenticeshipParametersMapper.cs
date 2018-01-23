using Esfa.Vacancy.Domain.Entities;

namespace Esfa.Vacancy.Application.Commands.CreateApprenticeship
{
    public class CreateApprenticeshipParametersMapper : ICreateApprenticeshipParametersMapper
    {
        private const int StandardLocationType = 1;
        public CreateApprenticeshipParameters MapFromRequest(CreateApprenticeshipRequest request)
        {
            return new CreateApprenticeshipParameters
            {
                Title = request.Title,
                ShortDescription = request.ShortDescription,
                Description = request.LongDescription,
                ApplicationClosingDate = request.ApplicationClosingDate,
                ExpectedStartDate = request.ExpectedStartDate,
                WorkingWeek = request.WorkingWeek,
                HoursPerWeek = request.HoursPerWeek,
                LocationTypeId = StandardLocationType, //This should change when more location type are introduced
                AddressLine1 = request.AddressLine1,
                AddressLine2 = request.AddressLine2,
                AddressLine3 = request.AddressLine3,
                AddressLine4 = request.AddressLine4,
                AddressLine5 = request.AddressLine5,
                Town = request.Town,
                Postcode = request.Postcode,
                NumberOfPositions = request.NumberOfPositions
            };
        }
    }
}