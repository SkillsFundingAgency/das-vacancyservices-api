using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Dapper;
using Esfa.Vacancy.Register.Domain.Entities;
using Esfa.Vacancy.Register.Domain.Repositories;
using Esfa.Vacancy.Register.Infrastructure.Settings;

namespace Esfa.Vacancy.Register.Infrastructure.Repositories
{
    public class StandardRepository : IStandardRepository
    {
        private readonly IProvideSettings _provideSettings;

        public StandardRepository(IProvideSettings provideSettings)
        {
            _provideSettings = provideSettings;
        }

        private const string GetStandardAndSectorIdsQuery = @"
            SELECT 
                LarsCode,
                StandardSectorId 
            FROM 
                Reference.Standard";

        public async Task<IEnumerable<StandardSector>> GetStandardsAndRespectiveSectorIdsAsync()
        {
            var connectionString =
                _provideSettings.GetSetting(ApplicationSettingConstants.AvmsPlusDatabaseConnectionStringKey);

            using (var sqlConn = new SqlConnection(connectionString))
            {
                await sqlConn.OpenAsync();

                var results =
                    await sqlConn.QueryAsync<StandardSector>(GetStandardAndSectorIdsQuery);

                return results;
            }
        }
    }
}
