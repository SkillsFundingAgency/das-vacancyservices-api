using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Esfa.Vacancy.Register.Application.Interfaces;
using Esfa.Vacancy.Register.Domain.Repositories;
using FluentValidation;
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

        public async Task<List<string>> Convert(IEnumerable<string> frameworksToConvert)
        {
            var convertedFrameworks = new List<string>();

            if (!frameworksToConvert.Any())
                return convertedFrameworks;

            var validFrameworks = await _frameworkCodeRepository.GetAsync();

            
            var validationFailures = new List<ValidationFailure>();

            frameworksToConvert.ToList().ForEach(frameworkToConvert =>
            {
                var trimmedFrameworkToConvert = frameworkToConvert.Trim();

                var validFramework = validFrameworks.FirstOrDefault(framework => 
                    framework.Equals(trimmedFrameworkToConvert, StringComparison.InvariantCultureIgnoreCase));

                if (validFramework == null)
                {
                    validationFailures.Add(new ValidationFailure("FrameworkCode", $"FrameworkCode {trimmedFrameworkToConvert} is invalid"));
                }
                else
                {
                    convertedFrameworks.Add($"{FrameworkPrefix}.{trimmedFrameworkToConvert}");
                }
            });

            if (validationFailures.Any())
                throw new ValidationException(validationFailures);

            return convertedFrameworks;
        }
    }
}