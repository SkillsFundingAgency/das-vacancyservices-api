﻿using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace Esfa.Vacancy.Api.Core
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
