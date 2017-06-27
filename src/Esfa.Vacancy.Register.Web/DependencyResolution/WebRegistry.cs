using System.Web;
using Esfa.Vacancy.Register.Web.Logging;
using SFA.DAS.NLog.Logger;

namespace Esfa.Vacancy.Register.Web.DependencyResolution
{
    public sealed class WebRegistry : StructureMap.Registry
    {
        public WebRegistry()
        {
            For<IRequestContext>().Use(x => new RequestContext(new HttpContextWrapper(HttpContext.Current)));
        }
    }
}