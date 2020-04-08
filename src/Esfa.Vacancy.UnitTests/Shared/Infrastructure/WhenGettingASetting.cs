using Esfa.Vacancy.Domain.Interfaces;
using Esfa.Vacancy.Infrastructure.Settings;
using FluentAssertions;
using Microsoft.Azure;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Esfa.Vacancy.UnitTests.Shared.Infrastructure
{
    public class WhenGettingASetting
    {
        [Test]
        public void ThenReturnSettingValue()
        {
            var settingKey = "MySetting";
            var settingValue = "hello world";
            var sut = new AppConfigSettingsProvider(new PropertiesSettingsProvider());
            Properties.Settings.Default[settingKey] = settingValue;

            var setting = sut.GetSetting(settingKey);

            setting.Should().Be(settingValue); 
        }

        [Test]
        public void AndSettingValueIsNull_ThenThrowException()
        {
            var settingKey = "MyBadSetting";
            var sut = new AppConfigSettingsProvider(new PropertiesSettingsProvider());

            Action act = () => sut.GetSetting(settingKey);
            act.ShouldThrow<ConfigurationErrorsException>();
        }
    }

    public class WhenGettingANullableSetting
    {
        [Test]
        public void ThenReturnSettingValue()
        {
            var settingKey = "MySetting";
            var settingValue = "hello world";
            var sut = new AppConfigSettingsProvider(new PropertiesSettingsProvider());
            Properties.Settings.Default[settingKey] = settingValue;

            var setting = sut.GetNullableSetting(settingKey);

            setting.Should().Be(settingValue);
        }

        [Test]
        public void AndSettingValueIsNull_ThenThrowException()
        {
            var settingKey = "MyBadSetting";
            var sut = new AppConfigSettingsProvider(new PropertiesSettingsProvider());

            sut.GetNullableSetting(settingKey).Should().BeNull();
        }
    }

    public class PropertiesSettingsProvider : IProvideSettings
    {
        public string GetSetting(string settingKey)
        {
            return GetNullableSetting(settingKey);
        }

        public string GetNullableSetting(string settingKey)
        {
            try
            {
                var value = Properties.Settings.Default[settingKey].ToString();
                return string.IsNullOrEmpty(value) ? null : value;
            }
            catch (SettingsPropertyNotFoundException)
            {
                return null;
            }
        }
    }
}
