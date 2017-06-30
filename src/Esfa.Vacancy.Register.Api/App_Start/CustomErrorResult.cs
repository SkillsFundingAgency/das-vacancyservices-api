using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace Esfa.Vacancy.Register.Api.App_Start
{
    public class CustomErrorResult : IHttpActionResult
    {
        private readonly HttpResponseMessage _httpResponseMessage;

        public CustomErrorResult(HttpRequestMessage request, HttpResponseMessage httpResponseMessage)
        {
            _httpResponseMessage = httpResponseMessage;
        }

        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult(_httpResponseMessage);
        }
    }
}
