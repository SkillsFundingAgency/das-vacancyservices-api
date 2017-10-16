using System;
using System.Globalization;

namespace Esfa.Vacancy.Register.Infrastructure.Settings
{
    public sealed class MachineSettings : IProvideSettings
    {
        public string GetSetting(string settingKey)
        {
            var userEnvironmentVariable = Environment.GetEnvironmentVariable(
                $"DAS_{settingKey.ToUpper(CultureInfo.InvariantCulture)}",
                EnvironmentVariableTarget.User);
            if (userEnvironmentVariable != null)
                return userEnvironmentVariable;

            return Environment.GetEnvironmentVariable(
                $"DAS_{settingKey.ToUpper(CultureInfo.InvariantCulture)}",
                EnvironmentVariableTarget.Machine);
        }

        public string GetNullableSetting(string settingKey)
        {
            return GetSetting(settingKey);
        }
    }
}
