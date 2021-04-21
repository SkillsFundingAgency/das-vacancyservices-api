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

        public static explicit operator TrainingDetail(GetStandardsListItem source)
        {
            return new TrainingDetail
            {
                Title = source.Title,
                Level = source.Level,
                EffectiveTo = source.StandardDates?.EffectiveTo,
                TrainingCode = source.LarsCode.ToString()
            };
        }
    }

    public class StandardDate
    {
        public DateTime? LastDateStarts { get; set; }

        public DateTime? EffectiveTo { get; set; }

        public DateTime EffectiveFrom { get; set; }
    }
}