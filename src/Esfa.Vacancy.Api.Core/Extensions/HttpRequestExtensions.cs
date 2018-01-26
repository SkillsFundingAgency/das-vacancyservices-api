using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

namespace Esfa.Vacancy.Api.Core.Extensions
{
    public static class HttpRequestExtensions
    {
        public static string[] Headers =
        {
            Constants.RequestHeaderNames.UserId,
            Constants.RequestHeaderNames.UserEmail,
            Constants.RequestHeaderNames.UserNote,
            Constants.RequestHeaderNames.ProviderUkprn
        };

        public static Dictionary<string, string> GetApimUserContextHeaders(this HttpRequestMessage request)
        {
            var result = new Dictionary<string, string>();

            foreach (var header in Headers)
            {
                var value = request.GetHeaderValue(header);
                result.Add(header, value);
            }

            return result;
        }

        public static string GetHeaderValue(this HttpRequestMessage request, string header)
        {
            IEnumerable<string> values;
            var found = request.Headers.TryGetValues(header, out values);
            if (found)
            {
                return values.FirstOrDefault();
            }
            return null;
        }
    }
}