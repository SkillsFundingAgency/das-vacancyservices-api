using Esfa.Vacancy.Api.Types;

namespace Esfa.Vacancy.Register.Api.Mappings
{
    public sealed class GeoCodedAddressMapper
    {
        public GeoCodedAddress MapToLocation(Domain.Entities.Address location, bool showAnonymousEmployerDetails)
        {
            if (showAnonymousEmployerDetails)
            {
                return new GeoCodedAddress
                {
                    Town = location.Town
                };
            }

            return new GeoCodedAddress
            {
                AddressLine1 = location.AddressLine1,
                AddressLine2 = location.AddressLine2,
                AddressLine3 = location.AddressLine3,
                AddressLine4 = location.AddressLine4,
                AddressLine5 = location.AddressLine5,
                GeoPoint = new GeoPoint()
                {
                    Latitude = location.Latitude,
                    Longitude = location.Longitude
                },
                PostCode = location.PostCode,
                Town = location.Town
            };
        }
    }
}