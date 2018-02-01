using Esfa.Vacancy.Api.AcceptanceTests.DependencyResolution;
using Esfa.Vacancy.Register.Api.DependencyResolution;
using Owin;
using StructureMap;
using System.Web.Http;

namespace Esfa.Vacancy.Api.AcceptanceTests
{
    public class TestStartup
    {
        internal static IContainer Container { get; private set; }

        public void Configuration(IAppBuilder app)
        {
            //Duplicated from API Global.asax.cs
            var config = new HttpConfiguration();

            // Configure IoC
            Container = IoC.Initialize();
            config.DependencyResolver = new StructureMapWebApiDependencyResolver(Container);

            Esfa.Vacancy.Register.Api.WebApiConfig.Register(config);

            app.UseWebApi(config);
        }
    }

    //public class TestStartup : Startup
    //{
    //    public override void Configuration(IAppBuilder app)
    //    {
    //        //Duplicated from API Global.asax.cs
    //        var config = new HttpConfiguration();

    //        //Configure IoC
    //        var container = IoC.Initialize();
    //        config.DependencyResolver = new StructureMapWebApiDependencyResolver(container);

    //        //Register API Key handler
    //        config.MessageHandlers.Add(new ApiKeyHandler(container.GetInstance<IAuthenticationService>()));

    //        WebApiConfig.Register(config);

    //        app.UseWebApi(config);
    //    }
    //}
}