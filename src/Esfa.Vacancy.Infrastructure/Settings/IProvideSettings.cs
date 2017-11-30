namespace Esfa.Vacancy.Infrastructure.Settings
{
    public interface IProvideSettings
    {
        string GetSetting(string settingKey);
        string GetNullableSetting(string settingKey);
    }
}
