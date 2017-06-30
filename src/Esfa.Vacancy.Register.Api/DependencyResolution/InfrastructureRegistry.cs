using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using Esfa.Vacancy.Register.Infrastructure.Settings;
using SFA.DAS.NLog.Logger;

namespace Esfa.Vacancy.Register.Api.DependencyResolution
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
