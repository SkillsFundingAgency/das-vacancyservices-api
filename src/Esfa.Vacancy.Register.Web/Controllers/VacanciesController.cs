using System;
using System.Net;
using System.Web.Http;
using Esfa.Vacancy.Register.Web.Attributes;
using SFA.DAS.NLog.Logger;
using Swashbuckle.Swagger.Annotations;

namespace Esfa.Vacancy.Register.Web.Controllers
{
    [RoutePrefix("api")]
    public class VacanciesController : ApiController
    {
        private readonly ILog _log;

        public VacanciesController(ILog log)
        {
            _log = log;
        }

        /// <summary>
        /// Check if vacancy exists
        /// </summary>
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
        /// Get a vacancy
        /// </summary>
        /// <returns></returns>
        [SwaggerOperation("Get")]
        [SwaggerResponse(HttpStatusCode.OK, "OK", typeof(Api.Types.Vacancy))]
        [SwaggerResponse(HttpStatusCode.NotFound)]
        [Route("vacancies/{reference}")]
        [ExceptionHandling]
        public Api.Types.Vacancy Get(int reference)
        {
            var response = new Api.Types.Vacancy { Reference = reference };

            if (response == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            response.Uri = Resolve(response.Reference);

            return response;
        }

        private string Resolve(int reference)
        {
            return Url.Link("DefaultApi", new { controller = "Vacancies", id = reference });
        }
    }
}
