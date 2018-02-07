using Esfa.Vacancy.Domain.Entities;

namespace Esfa.Vacancy.Application.Commands.CreateApprenticeship
{
    public class CreateApprenticeshipParametersMapper : ICreateApprenticeshipParametersMapper
    {
        private readonly IWageTypeMapper _wageTypeMapper;
        private const int StandardLocationType = 1;
        private const int NationwideLocationType = 3;
        public CreateApprenticeshipParametersMapper(IWageTypeMapper wageTypeMapper)
        {
            _wageTypeMapper = wageTypeMapper;
        }

        public CreateApprenticeshipParameters MapFromRequest(CreateApprenticeshipRequest request,
            EmployerInformation employerInformation)
        {
            var parameters = new CreateApprenticeshipParameters
            {
                Title = request.Title,
                ShortDescription = request.ShortDescription,
                Description = request.LongDescription,
                ApplicationClosingDate = request.ApplicationClosingDate,
                ExpectedStartDate = request.ExpectedStartDate,
                WorkingWeek = request.WorkingWeek,
                HoursPerWeek = request.HoursPerWeek,
                WageType = _wageTypeMapper.MapToLegacy(request.WageType),
                LocationTypeId = request.LocationType == LocationType.Nationwide ? NationwideLocationType : StandardLocationType,
                NumberOfPositions = request.NumberOfPositions,
                VacancyOwnerRelationshipId = employerInformation.VacancyOwnerRelationshipId.Value, //a value should always exist
                EmployerDescription = employerInformation.EmployerDescription,
                EmployersWebsite = employerInformation.EmployerWebsite,
                ContactName = request.ContactName,
                ContactEmail = request.ContactEmail
            };

            MapLocationFields(request, employerInformation, parameters);

            return parameters;
        }

        private static void MapLocationFields(CreateApprenticeshipRequest request, EmployerInformation employerInformation,
            CreateApprenticeshipParameters parameters)
        {
            if (request.LocationType == LocationType.OtherLocation)
            {
                parameters.AddressLine1 = request.AddressLine1;
                parameters.AddressLine2 = request.AddressLine2;
                parameters.AddressLine3 = request.AddressLine3;
                parameters.AddressLine4 = request.AddressLine4;
                parameters.AddressLine5 = request.AddressLine5;
                parameters.Town = request.Town;
                parameters.Postcode = request.Postcode;
            }
            else
            {
                parameters.AddressLine1 = employerInformation.AddressLine1;
                parameters.AddressLine2 = employerInformation.AddressLine2;
                parameters.AddressLine3 = employerInformation.AddressLine3;
                parameters.AddressLine4 = employerInformation.AddressLine4;
                parameters.AddressLine5 = employerInformation.AddressLine5;
                parameters.Town = employerInformation.Town;
                parameters.Postcode = employerInformation.Postcode;
            }
        }
    }
}