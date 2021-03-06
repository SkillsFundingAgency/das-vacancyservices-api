﻿using System.Configuration;
using Esfa.Vacancy.Domain.Interfaces;
using Microsoft.Azure;

namespace Esfa.Vacancy.Infrastructure.Settings
{
    public class AppConfigSettingsProvider : IProvideSettings
    {
        private readonly IProvideSettings _baseSettings;

        public AppConfigSettingsProvider(IProvideSettings baseSettings)
        {
            _baseSettings = baseSettings;
        }

        public string GetSetting(string settingKey)
        {
            var setting = CloudConfigurationManager.GetSetting(settingKey);

            if (string.IsNullOrWhiteSpace(setting))
            {
                setting = _baseSettings.GetSetting(settingKey);
            }

            if (string.IsNullOrEmpty(setting))
            {
                throw new ConfigurationErrorsException($"Setting with key {settingKey} is missing");
            }

            return setting;
        }

        public string GetNullableSetting(string settingKey)
        {
            var setting = CloudConfigurationManager.GetSetting(settingKey);

            if (string.IsNullOrEmpty(setting))
            {
                setting = _baseSettings.GetNullableSetting(settingKey);
            }

            return setting;
        }
    }
}
