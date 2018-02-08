namespace Esfa.Vacancy.Domain.Interfaces
{
    public interface IProvideSettings
    {
        string GetSetting(string settingKey);
        string GetNullableSetting(string settingKey);
    }
}
