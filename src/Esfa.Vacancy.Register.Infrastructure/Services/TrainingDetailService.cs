using System;
using System.Threading.Tasks;
using Esfa.Vacancy.Register.Domain.Entities;
using Esfa.Vacancy.Register.Domain.Interfaces;
using SFA.DAS.Apprenticeships.Api.Client;

namespace Esfa.Vacancy.Register.Infrastructure.Services
{
    public class TrainingDetailService : ITrainingDetailService
    {
        public async Task<Framework> GetFrameworkDetails(int code)
        {
            using (var client = new FrameworkCodeClient())
            {
                var framework = await client.GetAsync(code);
                if (framework == null) throw new Exception($"Framework: {code}");
                return new Framework() { Title = framework.Title };
            }
        }

        public async Task<Standard> GetStandardDetails(int code)
        {
            using (var client = new StandardApiClient())
            {
                var standard = await client.GetAsync(code);
                if (standard == null) throw new Exception($"Standard: {code}");
                return new Standard() { Title = standard.Title };
            }
        }
    }
}
