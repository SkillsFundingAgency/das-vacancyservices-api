using System.Web.Http;
using System.Web.Http.Filters;

namespace Esfa.Vacancy.Register.Api
{
    public class ApiFilterConfig
    {
        public static void RegisterFilters(HttpFilterCollection filters)
        {
            filters.Add(new AuthorizeAttribute());
        }
    }
}