using Esfa.Vacancy.Domain.Entities;

namespace Esfa.Vacancy.Application.Commands.CreateApprenticeship
{
    public class CreateApprenticeshipParametersMapper : ICreateApprenticeshipParametersMapper
    {
        private readonly IWageTypeMapper _wageTypeMapper;
        private const int StandardLocationType = 1;

        public CreateApprenticeshipParametersMapper(IWageTypeMapper wageTypeMapper)
        {
            _wageTypeMapper = wageTypeMapper;
        }
        
        public CreateApprenticeshipParameters MapFromRequest(CreateApprenticeshipRequest request,
            EmployerInformation employerInformation)
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
                WageType = _wageTypeMapper.MapToLegacy(request.WageType),
                LocationTypeId = StandardLocationType, //This should change when more location type are introduced
                AddressLine1 = request.AddressLine1,
                AddressLine2 = request.AddressLine2,
                AddressLine3 = request.AddressLine3,
                AddressLine4 = request.AddressLine4,
                AddressLine5 = request.AddressLine5,
                Town = request.Town,
                Postcode = request.Postcode,
                NumberOfPositions = request.NumberOfPositions,
                VacancyOwnerRelationshipId = employerInformation.VacancyOwnerRelationshipId.Value,
                EmployerDescription = employerInformation.EmployerDescription,
                EmployersWebsite = employerInformation.EmployerWebsite
            };
        }
    }
}