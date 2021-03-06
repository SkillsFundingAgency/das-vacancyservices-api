﻿using System.Threading.Tasks;
using Esfa.Vacancy.Api.Core.Validation;
using Esfa.Vacancy.Api.Types;
using Esfa.Vacancy.Application.Queries.GetTraineeshipVacancy;
using Esfa.Vacancy.Domain.Validation;
using Esfa.Vacancy.Register.Api.Mappings;
using MediatR;

namespace Esfa.Vacancy.Register.Api.Orchestrators
{
    public class GetTraineeshipVacancyOrchestrator
    {
        private readonly IMediator _mediator;
        private readonly ITraineeshipMapper _mapper;
        private readonly IValidationExceptionBuilder _validationExceptionBuilder;

        public GetTraineeshipVacancyOrchestrator(IMediator mediator, ITraineeshipMapper mapper, IValidationExceptionBuilder validationExceptionBuilder)
        {
            _mediator = mediator;
            _mapper = mapper;
            _validationExceptionBuilder = validationExceptionBuilder;
        }

        public async Task<TraineeshipVacancy> GetTraineeshipVacancyDetailsAsync(string id)
        {
            int parsedId;
            if (!int.TryParse(id, out parsedId))
            {
                throw _validationExceptionBuilder.Build(
                    ErrorCodes.GetTraineeship.VacancyReferenceNumberNotInt32,
                    ErrorMessages.GetTraineeship.VacancyReferenceNumberNotNumeric);
            }

            var response = await _mediator.Send(new GetTraineeshipVacancyRequest() { Reference = parsedId });
            var vacancy = _mapper.MapToTraineeshipVacancy(response.TraineeshipVacancy);

            return vacancy;
        }
    }
}
