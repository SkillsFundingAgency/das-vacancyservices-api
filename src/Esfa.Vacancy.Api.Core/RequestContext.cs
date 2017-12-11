using System.Web;
using SFA.DAS.NLog.Logger;

namespace Esfa.Vacancy.Api.Core
{
    public sealed class RequestContext : IRequestContext
    {
        public RequestContext(HttpContextBase context)
        {
            try
            {
                IpAddress = context?.Request.UserHostAddress;
                Url = context?.Request.RawUrl;
            }
            catch (HttpException)
            {

                // Happens on request that starts the application.
                IpAddress = string.Empty;
                Url = string.Empty;
            }
        }

        public string IpAddress { get; }

        public string Url { get; }
    }
}