using Microsoft.ApplicationInsights.Channel;

namespace Esfa.Vacancy.Register.Web.Logging
{
    public sealed class ApplicationInsightsInitializer : Microsoft.ApplicationInsights.Extensibility.ITelemetryInitializer
    {
        public void Initialize(ITelemetry telemetry)
        {
            telemetry.Context.Properties["Application"] = "Efa.Vacancy.Register";
        }
    }
}