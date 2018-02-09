using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using Esfa.Vacancy.Domain.Constants;
using Esfa.Vacancy.Domain.Interfaces;
using Esfa.Vacancy.Domain.Repositories;
using Esfa.Vacancy.Infrastructure.Caching;
using Esfa.Vacancy.Infrastructure.Factories;
using Esfa.Vacancy.Infrastructure.Repositories;
using Esfa.Vacancy.Infrastructure.Services;
using Esfa.Vacancy.Infrastructure.Settings;
using Nest;
using SFA.DAS.NLog.Logger;

namespace Esfa.Vacancy.Infrastructure
{
    public sealed class InfrastructureRegistry : StructureMap.Registry
    {
        private readonly IProvideSettings _provideSettings;

        public InfrastructureRegistry()
            : this(new AppConfigSettingsProvider(new MachineSettings()))
        { }

        public InfrastructureRegistry(IProvideSettings provideSettings)
        {
            _provideSettings = provideSettings;

            For<ILog>().Use(x => new NLogLogger(
                x.ParentType,
                x.GetInstance<IRequestContext>(),
                GetProperties())).AlwaysUnique();
            For<IProvideSettings>().Use(c => _provideSettings);
            For<IElasticClient>().Use(context => context.GetInstance<ElasticClientFactory>().GetClient());
            For<ICacheService>().Singleton().Use<AzureRedisCacheService>();

            For<IFrameworkCodeRepository>().Use<CachedFrameworkCodeRepository>()
                .Ctor<IFrameworkCodeRepository>().Is<FrameworkCodeRepository>();

            For<IStandardRepository>().Use<CachedStandardRepository>()
                .Ctor<IStandardRepository>().Is<StandardRepository>();

            RegisterCreateApprenticeshipService();
        }

        private IDictionary<string, object> GetProperties()
        {
            var properties = new Dictionary<string, object> { { "Version", GetVersion() } };
            return properties;
        }

        private static string GetVersion()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var fileVersionInfo = FileVersionInfo.GetVersionInfo(assembly.Location);
            return fileVersionInfo.ProductVersion;
        }

        private void RegisterCreateApprenticeshipService()
        {
            var isRunningUnderSandboxEnvironment =
                _provideSettings.GetNullableSetting(ApplicationSettingKeys.UseSandboxServices);

            if (string.IsNullOrWhiteSpace(isRunningUnderSandboxEnvironment))
            {
                For<ICreateApprenticeshipService>().Use<CreateApprenticeshipService>();
            }
            else
            {
                For<ICreateApprenticeshipService>().Use<CreateApprenticeshipSandboxService>();
            }
        }
    }
}
