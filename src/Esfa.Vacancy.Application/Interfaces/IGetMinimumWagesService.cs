using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Esfa.Vacancy.Domain.Entities;

namespace Esfa.Vacancy.Application.Interfaces
{
    public interface IGetMinimumWagesService
    {
        Task<IEnumerable<WageRange>> GetAllWagesAsync();

        Task<WageRange> GetWageRangeAsync(DateTime expectedStartDate);
    }
}