using System.Threading.Tasks;
using Esfa.Vacancy.Register.Domain.Entities;
using Esfa.Vacancy.Register.Domain.Interfaces;
using Esfa.Vacancy.Register.Infrastructure.Settings;
using SFA.DAS.Apprenticeships.Api.Client;
using SFA.DAS.Apprenticeships.Api.Types.Exceptions;
using SFA.DAS.NLog.Logger;

namespace Esfa.Vacancy.Register.Infrastructure.Services
{
    public class TrainingDetailService : ITrainingDetailService
    {
        private readonly ILog _logger;
        private readonly string _dasApiBaseUrl;

        public TrainingDetailService(IProvideSettings provideSettings, ILog logger)
        {
            _logger = logger;
            _dasApiBaseUrl =
                provideSettings.GetSetting(ApplicationSettingConstants.DasApprenticeshipInfoApiBaseUrl);
        }

        public async Task<Framework> GetFrameworkDetailsAsync(int code)
        {
            using (var client = new FrameworkCodeClient(_dasApiBaseUrl))
            {
                try
                {
                    var framework = await client.GetAsync(code);
                    return new Framework() { Title = framework.Title, Code = code, Uri = framework.Uri };
                }
                catch (EntityNotFoundException ex)
                {
                    _logger.Warn(ex, $"Framework details not found for {code}");
                    return null;
                }
            }
        }

        public async Task<Standard> GetStandardDetailsAsync(int code)
        {
            using (var client = new StandardApiClient(_dasApiBaseUrl))
            {
                try
                {
                    var standard = await client.GetAsync(code);
                    return new Standard() { Title = standard.Title, Code = code, Uri = standard.Uri };
                }
                catch (EntityNotFoundException ex)
                {
                    _logger.Warn(ex, $"Standard details not found for {code}");
                    return null;
                }
            }
        }
    }
}
