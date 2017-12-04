namespace Esfa.Vacancy.Manage.Api.DependencyResolution
{
    using System.Web;

    using Esfa.Vacancy.Manage.Api.App_Start;

    using StructureMap.Web.Pipeline;

    public class StructureMapScopeModule : IHttpModule
    {
        #region Public Methods and Operators

        public void Dispose()
        {
        }

        public void Init(HttpApplication context)
        {
            context.BeginRequest += (sender, e) => StructuremapConfig.StructureMapDependencyScope.CreateNestedContainer();
            context.EndRequest += (sender, e) =>
            {
                HttpContextLifecycle.DisposeAndClearAll();
                StructuremapConfig.StructureMapDependencyScope.DisposeNestedContainer();
            };
        }

        #endregion
    }
}