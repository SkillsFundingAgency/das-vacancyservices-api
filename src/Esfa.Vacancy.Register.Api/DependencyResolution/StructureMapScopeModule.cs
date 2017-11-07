namespace Esfa.Vacancy.Register.Api.DependencyResolution {
    using System.Web;
    using StructureMap.Web.Pipeline;

    public class StructureMapScopeModule : IHttpModule {
        #region Public Methods and Operators

        public void Dispose() {
        }

        public void Init(HttpApplication context) {
            context.BeginRequest += (sender, e) => StructureMapConfig.StructureMapDependencyScope.CreateNestedContainer();
            context.EndRequest += (sender, e) => {
                HttpContextLifecycle.DisposeAndClearAll();
                StructureMapConfig.StructureMapDependencyScope.DisposeNestedContainer();
            };
        }

        #endregion
    }
}