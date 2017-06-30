using System;

namespace Esfa.Vacancy.Register.Infrastructure.Settings
{
    public interface IProvideSettings
    {
        string GetSetting(string settingKey);
        string GetNullableSetting(string settingKey);
    }
}
