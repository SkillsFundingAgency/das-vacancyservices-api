﻿using System;
using Esfa.Vacancy.Domain.Entities;

namespace Esfa.Vacancy.Infrastructure.InnerApi.Responses
{
    public class GetFrameworksListItem
    {
        public string Title { get; set; }
        public int FrameworkCode { get; set; }
        public int Level { get; set; }
        public DateTime? EffectiveTo { get; set; }

        public static explicit operator TrainingDetail(GetFrameworksListItem source)
        {
            return new TrainingDetail
            {
                Title = source.Title,
                Level = source.Level,
                EffectiveTo = source.EffectiveTo,
                TrainingCode = source.FrameworkCode.ToString()
            };
        }
    }
}