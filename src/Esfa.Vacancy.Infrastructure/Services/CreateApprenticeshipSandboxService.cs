using System.Threading.Tasks;
using Esfa.Vacancy.Domain.Entities;
using Esfa.Vacancy.Domain.Interfaces;
using Newtonsoft.Json;
using SFA.DAS.NLog.Logger;

namespace Esfa.Vacancy.Infrastructure.Services
{
    public class CreateApprenticeshipSandboxService : ICreateApprenticeshipService
    {
        private readonly ILog _logger;

        public CreateApprenticeshipSandboxService(ILog logger)
        {
            _logger = logger;
        }

        public async Task<int> CreateApprenticeshipAsync(CreateApprenticeshipParameters parameters)
        {
            var serialized = JsonConvert.SerializeObject(parameters);
            _logger.Info($"Sandbox Request for Create Apprenticeship : {serialized}");
            return await Task.FromResult(0);
        }
    }
}