using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using Esfa.Vacancy.Application.Interfaces;
using Esfa.Vacancy.Domain.Constants;
using Esfa.Vacancy.Domain.Interfaces;
using Esfa.Vacancy.Infrastructure.Caching;
using Esfa.Vacancy.Infrastructure.Services;
using Esfa.Vacancy.Infrastructure.Settings;
using SFA.DAS.NLog.Logger;

namespace Esfa.Vacancy.Infrastructure.Ioc
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
                x.GetInstance<IWebLoggingContext>(),
                GetProperties())).AlwaysUnique();
            For<IProvideSettings>().Use(c => _provideSettings);

            For<ICacheService>().Singleton().Use<AzureRedisCacheService>();

            For<IGetMinimumWagesService>().Use<GetMinimumWagesService>();

            For<ITrainingDetailService>().Use<CachedTrainingDetailService>()
                .Ctor<ITrainingDetailService>().Is<TrainingDetailService>();

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

            if ("yes".Equals(isRunningUnderSandboxEnvironment, StringComparison.OrdinalIgnoreCase))
            {
                For<ICreateApprenticeshipService>().Use<CreateApprenticeshipSandboxService>();
            }
            else
            {
                For<ICreateApprenticeshipService>().Use<CreateApprenticeshipService>();
            }
        }
    }
}
