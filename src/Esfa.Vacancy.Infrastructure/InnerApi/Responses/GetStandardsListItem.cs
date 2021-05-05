using System;
using Esfa.Vacancy.Domain.Entities;

namespace Esfa.Vacancy.Infrastructure.InnerApi.Responses
{
    public class GetStandardsListItem
    {
        public int LarsCode { get; set; }
        public string Title { get; set; }
        public int Level { get; set; }
        public StandardDate StandardDates { get; set; }

        public static implicit operator TrainingDetail(GetStandardsListItem source)
        {
            return new TrainingDetail
            {
                Title = source.Title,
                Level = source.Level,
                EffectiveTo = source.StandardDates?.EffectiveTo,
                TrainingCode = source.LarsCode.ToString()
            };
        }

        public static implicit operator Standard(GetStandardsListItem source)
        {
            return new Standard
            {
                Title = source.Title,
                Code = source.LarsCode
            };
        }
    }

    public class StandardDate
    {
        public DateTime? EffectiveTo { get; set; }
    }
}