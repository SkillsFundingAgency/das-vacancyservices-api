namespace Esfa.Vacancy.Api.Types
{
    public struct GeoPoint
    {
        public GeoPoint(double latitude, double longitude)
        {
            Longitude = (decimal)longitude;
            Latitude = (decimal)latitude;
        }

        /// <summary>
        /// The longitude.
        /// </summary>
        public decimal? Longitude { get; set; }

        /// <summary>
        /// The latitude.
        /// </summary>
        public decimal? Latitude { get; set; }
    }
}
