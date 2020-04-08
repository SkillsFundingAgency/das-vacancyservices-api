using System;
using System.Globalization;
using Esfa.Vacancy.Domain.Interfaces;

namespace Esfa.Vacancy.Infrastructure.Settings
{
    public sealed class MachineSettings : IProvideSettings
    {
        const string _prefix = "raa_";
        public string GetSetting(string settingKey)
        {
            var userEnvironmentVariable = Environment.GetEnvironmentVariable(
                $"{_prefix}{settingKey.ToLower(CultureInfo.InvariantCulture)}",
                EnvironmentVariableTarget.User);
            if (userEnvironmentVariable != null)
                return userEnvironmentVariable;

            return Environment.GetEnvironmentVariable(
                $"{_prefix}{settingKey.ToLower(CultureInfo.InvariantCulture)}",
                EnvironmentVariableTarget.Machine);
        }

        public string GetNullableSetting(string settingKey)
        {
            return GetSetting(settingKey);
        }
    }
}
