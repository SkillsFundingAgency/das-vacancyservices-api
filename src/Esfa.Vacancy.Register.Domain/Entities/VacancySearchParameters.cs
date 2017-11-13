using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Esfa.Vacancy.Register.Domain.Entities
{
    public class VacancySearchParameters
    {
        public List<string> FrameworkLarsCodes { get; set; } = new List<string>();
        public List<string> StandardLarsCodes { get; set; } = new List<string>();
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
        public DateTime? FromDate { get; set; }
        public string LocationType { get; set; }
        public double? Longitude { get; set; }
        public double? Latitude { get; set; }
        public int? DistanceInMiles { get; set; }
        public bool HasGeoSearchFields => Latitude.HasValue && Longitude.HasValue && DistanceInMiles.HasValue;

        public override string ToString()
        {
            var result = new StringBuilder();
            if (FrameworkLarsCodes.Any())
            {
                result.AppendLine($"{nameof(FrameworkLarsCodes)}: {FrameworkLarsCodes.Aggregate((i, j) => i + ", " + j)}");
            }
            if (StandardLarsCodes.Any())
            {
                result.AppendLine($"{nameof(StandardLarsCodes)}: {StandardLarsCodes.Aggregate((i, j) => i + ", " + j)}");
            }

            result.AppendLine($"{nameof(PageSize)}: {PageSize}");
            result.AppendLine($"{nameof(PageNumber)}: {PageNumber}");
            if (FromDate.HasValue)
                result.AppendLine($"{nameof(FromDate)}: {FromDate}");
            if (!string.IsNullOrWhiteSpace(LocationType))
                result.AppendLine($"{nameof(LocationType)}: {LocationType}");
            if (Latitude.HasValue)
                result.AppendLine($"{nameof(Latitude)}: {Latitude}");
            if (Longitude.HasValue)
                result.AppendLine($"{nameof(Longitude)}: {Longitude}");
            if (DistanceInMiles.HasValue)
                result.AppendLine($"{nameof(DistanceInMiles)}: {DistanceInMiles}");

            return result.ToString();
        }
    }
}
