﻿using Esfa.Vacancy.Register.Api.Attributes;
using Esfa.Vacancy.Register.Infrastructure.Repositories;
using SFA.DAS.NLog.Logger;
using Swashbuckle.Swagger.Annotations;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;

namespace Esfa.Vacancy.Register.Api.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [RoutePrefix("api/vacancies")]
    public class VacanciesController : ApiController
    {
        private readonly ILog _log;
        private readonly IVacancyRepository _vacancyRepository;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="log"></param>
        /// <param name="vacancyRepository"></param>
        public VacanciesController(ILog log, IVacancyRepository vacancyRepository)
        {
            _log = log;
            _vacancyRepository = vacancyRepository;
        }

        /// <summary>
        /// Check if a vacancy exists
        /// </summary>
        /// <param name="id"></param>
        [SwaggerOperation("Head")]
        [SwaggerResponse(HttpStatusCode.NoContent)]
        [SwaggerResponse(HttpStatusCode.NotFound)]
        [ExceptionHandling]
        public async Task Head(int id)
        {
            await Get(id);
        }

        /// <summary>
        /// Get a vacancy by the public vacancy reference identifier
        /// </summary>
        /// <param name="id">The public vacancy reference identifier</param>
        /// <returns>A vacancy for an apprenticeship or a traineeship</returns>
        [SwaggerOperation("Get")]
        [SwaggerResponse(HttpStatusCode.OK, "OK", typeof(Vacancy.Api.Types.Vacancy))]
        [SwaggerResponse(HttpStatusCode.NotFound)]
        [ExceptionHandling]
        public async Task<Vacancy.Api.Types.Vacancy> Get(int id)
        {
            var response = await _vacancyRepository.GetVacancyByReferenceNumberAsync(id);

            if (response == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            response.Url = Resolve(response.Reference);

            return response;
        }

        private string Resolve(int reference)
        {
            return Url.Link("DefaultApi", new { controller = "Vacancies", id = reference });
        }
    }
}
