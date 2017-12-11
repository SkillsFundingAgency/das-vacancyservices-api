using System.Web;
using Esfa.Vacancy.Api.Core;
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
            For<SingleInstanceFactory>().Use<SingleInstanceFactory>(ctx => ctx.GetInstance);
            For<MultiInstanceFactory>().Use<MultiInstanceFactory>(ctx => ctx.GetAllInstances);
            For<IMediator>().Use<Mediator>();
        }
    }
}