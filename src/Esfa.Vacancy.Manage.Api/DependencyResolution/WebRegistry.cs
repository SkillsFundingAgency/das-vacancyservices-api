using System.Web;
using Esfa.Vacancy.Manage.Api.Logging;
using MediatR;
using SFA.DAS.NLog.Logger;

namespace Esfa.Vacancy.Manage.Api.DependencyResolution
{
    public sealed class WebRegistry : StructureMap.Registry
    {
        public WebRegistry()
        {
            For<IRequestContext>().Use(x => new RequestContext(new HttpContextWrapper(HttpContext.Current)));

            // mediatr
            For<SingleInstanceFactory>().Use<SingleInstanceFactory>(ctx => t => ctx.GetInstance(t));
            For<MultiInstanceFactory>().Use<MultiInstanceFactory>(ctx => t => ctx.GetAllInstances(t));
            For<IMediator>().Use<Mediator>();
        }
    }
}