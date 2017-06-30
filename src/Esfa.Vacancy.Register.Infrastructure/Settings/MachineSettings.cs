using System;
using System.Globalization;

namespace Esfa.Vacancy.Register.Infrastructure.Settings
{
    public sealed class MachineSettings : IProvideSettings
    {
        public string GetSetting(string settingKey)
        {
            return Environment.GetEnvironmentVariable($"DAS_{settingKey.ToUpper(CultureInfo.InvariantCulture)}", EnvironmentVariableTarget.User);
        }

        public string GetNullableSetting(string settingKey)
        {
            return GetSetting(settingKey);
        }
    }
}
