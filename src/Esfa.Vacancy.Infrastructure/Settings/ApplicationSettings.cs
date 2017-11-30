namespace Esfa.Vacancy.Infrastructure.Settings
{
    public sealed class ApplicationSettings
    {
        private readonly IProvideSettings _settings;

        public ApplicationSettings(IProvideSettings settings)
        {
            _settings = settings;
        }

        public string SomeSetting => _settings.GetSetting("SomeSetting");
    }
}
