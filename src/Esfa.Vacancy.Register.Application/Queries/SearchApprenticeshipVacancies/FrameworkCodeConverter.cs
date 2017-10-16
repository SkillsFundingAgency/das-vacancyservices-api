using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Esfa.Vacancy.Register.Application.Interfaces;
using Esfa.Vacancy.Register.Domain.Repositories;
using FluentValidation.Results;

namespace Esfa.Vacancy.Register.Application.Queries.SearchApprenticeshipVacancies
{
    public class FrameworkCodeConverter : IFrameworkCodeConverter
    {
        private readonly IFrameworkCodeRepository _frameworkCodeRepository;
        private const string FrameworkPrefix = "FW";

        public FrameworkCodeConverter(IFrameworkCodeRepository frameworkCodeRepository)
        {
            _frameworkCodeRepository = frameworkCodeRepository;
        }

        public async Task<SubCategoryConversionResult> ConvertAsync(List<string> frameworks)
        {
            var result = new SubCategoryConversionResult();

            if (!frameworks.Any())
                return result;

            var validFrameworks = await _frameworkCodeRepository.GetAsync();

            frameworks.ForEach(frameworkToConvert =>
            {
                var trimmedFrameworkToConvert = frameworkToConvert.Trim();

                var validFramework = validFrameworks.FirstOrDefault(framework => 
                    framework.Equals(trimmedFrameworkToConvert, StringComparison.InvariantCultureIgnoreCase));

                if (validFramework == null)
                {
                    result.ValidationFailures.Add(new ValidationFailure("FrameworkCode", $"FrameworkCode {trimmedFrameworkToConvert} is invalid"));
                }
                else
                {
                    result.SubCategoryCodes.Add($"{FrameworkPrefix}.{trimmedFrameworkToConvert}");
                }
            });
            
            return result;
        }
    }
}