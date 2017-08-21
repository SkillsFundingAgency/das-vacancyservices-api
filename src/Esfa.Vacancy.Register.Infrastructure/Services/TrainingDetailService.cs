using System;
using System.Threading.Tasks;
using Esfa.Vacancy.Register.Domain.Entities;
using Esfa.Vacancy.Register.Domain.Interfaces;
using Esfa.Vacancy.Register.Infrastructure.Settings;
using SFA.DAS.Apprenticeships.Api.Client;

namespace Esfa.Vacancy.Register.Infrastructure.Services
{
    public class TrainingDetailService : ITrainingDetailService
    {
        private readonly string _dasApiBaseUrl;

        public TrainingDetailService(IProvideSettings provideSettings)
        {
            _dasApiBaseUrl =
                provideSettings.GetSetting(ApplicationSettingConstants.DasApprenticeshipInfoApiBaseUrl);
        }

        public async Task<Framework> GetFrameworkDetailsAsync(int code)
        {
            using (var client = new FrameworkCodeClient(_dasApiBaseUrl))
            {
                var framework = await client.GetAsync(code);
                if (framework == null) throw new Exception($"Framework: {code}");
                return new Framework() { Title = framework.Title };
            }
        }

        public async Task<Standard> GetStandardDetailsAsync(int code)
        {
            using (var client = new StandardApiClient(_dasApiBaseUrl))
            {
                var standard = await client.GetAsync(code);
                if (standard == null) throw new Exception($"Standard: {code}");
                return new Standard() { Title = standard.Title };
            }
        }
    }
}
