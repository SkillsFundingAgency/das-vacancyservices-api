using System;
using System.Net;
using System.Web.Http;
using Esfa.Vacancy.Register.Api.Attributes;
using Esfa.Vacancy.Register.Application.Queries.GetVacancy;
using MediatR;
using SFA.DAS.NLog.Logger;
using Swashbuckle.Swagger.Annotations;

namespace Esfa.Vacancy.Register.Api.Controllers
{
    [RoutePrefix("api")]
    public class VacanciesController : ApiController
    {
        private readonly ILog _log;
        private readonly IMediator _mediator;

        public VacanciesController(ILog log, IMediator mediator)
        {
            _log = log;
            _mediator = mediator;
        }

        /// <summary>
        /// Check if a vacancy exists
        /// </summary>
        /// <param name="reference"></param>
        [SwaggerOperation("Head")]
        [SwaggerResponse(HttpStatusCode.NoContent)]
        [SwaggerResponse(HttpStatusCode.NotFound)]
        [Route("vacancies/{reference}")]
        [ExceptionHandling]
        public void Head(int reference)
        {
            Get(reference);
        }

        /// <summary>
        /// Get a vacancy by the public vacancy reference identifier
        /// </summary>
        /// <param name="reference">The public vacancy reference identifier</param>
        /// <returns>A vacancy for an apprenticeship or a traineeship</returns>
        [SwaggerOperation("Get")]
        [SwaggerResponse(HttpStatusCode.OK, "OK", typeof(Vacancy.Api.Types.Vacancy))]
        [SwaggerResponse(HttpStatusCode.NotFound)]
        [Route("vacancies/{reference}")]
        [ExceptionHandling]
        public Vacancy.Api.Types.Vacancy Get(int reference)
        {
            //todo: add mediatr call here, eg: 
            //var vacancy = _mediator.Send(new GetVacancyRequest {Reference = reference});

            var response = new Vacancy.Api.Types.Vacancy {Reference = reference};

            if (response == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            response.Url = Resolve(response.Reference);

            return response;
        }

        private string Resolve(int reference)
        {
            return Url.Link("DefaultApi", new {controller = "Vacancies", id = reference});
        }
    }
}
