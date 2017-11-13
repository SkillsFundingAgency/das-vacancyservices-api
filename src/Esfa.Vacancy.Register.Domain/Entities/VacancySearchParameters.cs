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
                result.Append($"{nameof(FrameworkLarsCodes)}: {FrameworkLarsCodes.Aggregate((i, j) => i + ", " + j)}");
                result.Append(Environment.NewLine);
            }
            if (StandardLarsCodes.Any())
            {
                result.Append($"{nameof(StandardLarsCodes)}: {StandardLarsCodes.Aggregate((i, j) => i + ", " + j)}");
                result.Append(Environment.NewLine);
            }

            result.Append($"{nameof(PageSize)}: {PageSize}{Environment.NewLine}");
            result.Append($"{nameof(PageNumber)}: {PageNumber}{Environment.NewLine}");
            if (FromDate.HasValue)
                result.Append($"{nameof(FromDate)}: {FromDate}{Environment.NewLine}");
            if (!string.IsNullOrWhiteSpace(LocationType))
                result.Append($"{nameof(LocationType)}: {LocationType}{Environment.NewLine}");
            if (Latitude.HasValue)
                result.Append($"{nameof(Latitude)}: {Latitude}{Environment.NewLine}");
            if (Longitude.HasValue)
                result.Append($"{nameof(Longitude)}: {Longitude}{Environment.NewLine}");
            if (DistanceInMiles.HasValue)
                result.Append($"{nameof(DistanceInMiles)}: {DistanceInMiles}{Environment.NewLine}");

            return result.ToString();
        }
    }
}
