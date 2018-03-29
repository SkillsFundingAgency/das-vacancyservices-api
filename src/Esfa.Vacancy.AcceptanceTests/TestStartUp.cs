using System.Web.Http;
using Esfa.Vacancy.Register.Api;
using Esfa.Vacancy.Register.Api.DependencyResolution;
using Owin;

namespace Esfa.Vacancy.AcceptanceTests
{
    public class TestStartUp
    {
        public void Configuration(IAppBuilder appBuilder)
        {
            var config = new HttpConfiguration();

            var container = IoC.Initialize();

            config.DependencyResolver = new StructureMapWebApiDependencyResolver(container);

            WebApiConfig.Register(config);

            appBuilder.UseWebApi(config);
        }
    }
}