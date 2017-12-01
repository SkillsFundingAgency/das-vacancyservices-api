using System.Net;
using System.Web.Http;
using Esfa.Vacancy.Api.Types;
using Swashbuckle.Swagger.Annotations;

namespace Esfa.Vacancy.Manage.Api.Controllers
{
    [RoutePrefix("api/v1/apprenticeships")]
    public class CreateApprenticeshipController : ApiController
    {
        /// <summary>
        /// Creates the specified apprenticeship vacancy.
        /// </summary>
        [HttpPost]
        [AllowAnonymous]
        [SwaggerOperation("CreateApprenticeshipVacancy", Tags = new[] { "Apprenticeships" })]
        [SwaggerResponse(HttpStatusCode.OK, "OK", typeof(CreateApprecticeshipResponse))]
        [SwaggerResponse(HttpStatusCode.BadRequest, "Failed request validation", typeof(BadRequestContent))]
        public IHttpActionResult Create([FromBody]CreateApprenticeshipParameters createApprenticeshipParameters)
        {
            return Ok(new CreateApprecticeshipResponse());
        }
    }
}
