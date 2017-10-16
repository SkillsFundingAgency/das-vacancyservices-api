using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using Esfa.Vacancy.Register.Infrastructure.Factories;
using Esfa.Vacancy.Register.Infrastructure.Settings;
using Nest;
using SFA.DAS.NLog.Logger;

namespace Esfa.Vacancy.Register.Infrastructure
{
    public sealed class InfrastructureRegistry : StructureMap.Registry
    {
        public InfrastructureRegistry()
        {
            For<ILog>().Use(x => new NLogLogger(
                x.ParentType,
                x.GetInstance<IRequestContext>(),
                GetProperties())).AlwaysUnique();
            For<IProvideSettings>().Use(c => new AppConfigSettingsProvider(new MachineSettings()));
            For<IElasticClient>().Use(context => context.GetInstance<ElasticClientFactory>().GetClient());
        }

        private IDictionary<string, object> GetProperties()
        {
            var properties = new Dictionary<string, object>();
            properties.Add("Version", GetVersion());
            return properties;
        }

        private string GetVersion()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var fileVersionInfo = FileVersionInfo.GetVersionInfo(assembly.Location);
            return fileVersionInfo.ProductVersion;
        }
    }
}
