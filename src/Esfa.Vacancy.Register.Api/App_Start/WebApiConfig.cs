using System.Web.Http;
using System.Web.Http.ExceptionHandling;
using Esfa.Vacancy.Register.Api.App_Start;
using Newtonsoft.Json;

namespace Esfa.Vacancy.Register.Api
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // JSON formatters
            config.Formatters.JsonFormatter.SerializerSettings = new JsonSerializerSettings {
                NullValueHandling = NullValueHandling.Ignore,
                DateFormatHandling = DateFormatHandling.IsoDateFormat,
                DateTimeZoneHandling = DateTimeZoneHandling.Utc
            };

            ApiFilterConfig.RegisterFilters(config.Filters);

            GlobalConfiguration.Configuration.Formatters.JsonFormatter.SerializerSettings.Converters.Add(new StrictEnumConverter());
            
            // Web API routes
            config.MapHttpAttributeRoutes();
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            var validationBadRequestBuilder = (IValidationBadRequestBuilder)GlobalConfiguration.Configuration.DependencyResolver.GetService(typeof(IValidationBadRequestBuilder));
            config.Services.Replace(typeof(IExceptionHandler), new VacancyApiExceptionHandler(validationBadRequestBuilder));
        }
    }
}
