using System.Web;
using System.Web.Mvc;

namespace Esfa.Vacancy.Manage.Api
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
