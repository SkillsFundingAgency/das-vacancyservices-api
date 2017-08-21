using System;
using System.Threading.Tasks;
using Esfa.Vacancy.Register.Application.Exceptions;
using Esfa.Vacancy.Register.Domain.Interfaces;
using Esfa.Vacancy.Register.Domain.Repositories;
using FluentValidation;
using MediatR;
using SFA.DAS.NLog.Logger;

namespace Esfa.Vacancy.Register.Application.Queries.GetVacancy
{
    public class GetVacancyQueryHandler : IAsyncRequestHandler<GetVacancyRequest, GetVacancyResponse>
    {
        private readonly AbstractValidator<GetVacancyRequest> _validator;
        private readonly IVacancyRepository _vacancyRepository;
        private readonly ILog _logger;
        private readonly ITrainingDetailService _trainingDetailService;

        public GetVacancyQueryHandler(AbstractValidator<GetVacancyRequest> validator,
            IVacancyRepository vacancyRepository,
            ILog logger,
            ITrainingDetailService trainingDetailService)
        {
            _validator = validator;
            _vacancyRepository = vacancyRepository;
            _logger = logger;
            _trainingDetailService = trainingDetailService;
        }

        public async Task<GetVacancyResponse> Handle(GetVacancyRequest message)
        {
            _logger.Info($"Getting Vacancy Details, Vacancy: {message.Reference}");

            var validationResult = _validator.Validate(message);

            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            var vacancy = await _vacancyRepository.GetVacancyByReferenceNumberAsync(message.Reference);

            if (vacancy == null) throw new ResourceNotFoundException($"Vacancy: {message.Reference}");

            if (vacancy.FrameworkCode.HasValue)
            {
                var framework = await _trainingDetailService.GetFrameworkDetailsAsync(vacancy.FrameworkCode.Value);
                vacancy.FrameworkTitle = framework.Title;
            }
            else if (vacancy.StandardCode.HasValue)
            {
                var standard = await _trainingDetailService.GetStandardDetailsAsync(vacancy.StandardCode.Value);
                vacancy.StandardTitle = standard.Title;
            }
            else
            {
                throw new Exception($"Invalid training type on Vacancy {message.Reference}");
            }

            return new GetVacancyResponse { Vacancy = vacancy };
        }
    }
}
