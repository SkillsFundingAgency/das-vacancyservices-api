using System.Web.Http;
using Esfa.Vacancy.Register.Api;
using Esfa.Vacancy.Register.Api.DependencyResolution;
using Owin;
using StructureMap;

namespace Esfa.Vacancy.AcceptanceTests
{
    public class TestStartUp
    {
        internal static IContainer Container { get; private set; }

        public void Configuration(IAppBuilder appBuilder)
        {
            var config = new HttpConfiguration();

            Container = IoC.Initialize();
            config.DependencyResolver = new StructureMapWebApiDependencyResolver(Container);

            WebApiConfig.Register(config);

            appBuilder.UseWebApi(config);
        }
    }
}