using System.Web.Http.Filters;

namespace Esfa.Vacancy.Manage.Api
{
    public class ApiFilterConfig
    {
        public static void RegisterFilters(HttpFilterCollection filters)
        {
            //filters.Add(new AuthorizeAttribute());
        }
    }
}