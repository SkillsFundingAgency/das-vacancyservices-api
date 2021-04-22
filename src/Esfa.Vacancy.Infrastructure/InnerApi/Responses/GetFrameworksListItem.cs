using System;
using Esfa.Vacancy.Domain.Entities;

namespace Esfa.Vacancy.Infrastructure.InnerApi.Responses
{
    public class GetFrameworksListItem
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string FrameworkName { get; set; }
        public int FrameworkCode { get; set; }
        public int Level { get; set; }
        public DateTime? EffectiveTo { get; set; }

        public static implicit operator TrainingDetail(GetFrameworksListItem source)
        {
            return new TrainingDetail
            {
                Title = source.Title,
                Level = source.Level,
                EffectiveTo = source.EffectiveTo,
                TrainingCode = source.Id,
                FrameworkCode = source.FrameworkCode,
                FrameworkName = source.FrameworkName
            };
        }

        public static implicit operator Framework(GetFrameworksListItem source)
        {
            return new Framework
            {
                Title = source.FrameworkName,
                Code = source.FrameworkCode
            };
        }
    }
}