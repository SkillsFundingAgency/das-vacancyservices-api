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
        private readonly IFrameworkRepository _frameworkRepository;

        public FrameworkCodeConverter(IFrameworkRepository frameworkRepository)
        {
            _frameworkRepository = frameworkRepository;
        }

        public async Task<List<string>> Convert(IEnumerable<string> frameworksToConvert)
        {
            var validFrameworks = await _frameworkRepository.GetFrameworksAsync();

            var convertedFrameworks = new List<string>();
            var validationFailures = new List<ValidationFailure>();

            frameworksToConvert.ToList().ForEach(frameworkToConvert =>
            {
                var validFramework = validFrameworks.FirstOrDefault(framework => framework.Code == int.Parse(frameworkToConvert));
                if (validFramework == null)
                {
                    validationFailures.Add(new ValidationFailure("FrameworkCode", $"FrameworkCode {frameworkToConvert} is invalid"));
                }
                else
                {
                    convertedFrameworks.Add($"FW.{frameworkToConvert.Trim()}");
                }
            });

            if (validationFailures.Any())
                throw new ValidationException(validationFailures);

            return convertedFrameworks;
        }
    }
}