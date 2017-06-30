using System;
using FluentValidation;
using MediatR;

namespace Esfa.Vacancy.Register.Application.Queries.GetVacancy
{
    public class GetVacancyQueryHandler : IRequestHandler<GetVacancyRequest, GetVacancyResponse>
    {
        private readonly AbstractValidator<GetVacancyRequest> _validator;

        public GetVacancyQueryHandler(AbstractValidator<GetVacancyRequest> validator)
        {
            _validator = validator;
        }

        public GetVacancyResponse Handle(GetVacancyRequest message)
        {
            var validationResult = _validator.Validate(message);

            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            //todo: add call to repo to retrieve from DB

            var vacancy = new Domain.Entities.Vacancy {Reference = message.Reference};

            return new GetVacancyResponse {Vacancy = vacancy};
        }
    }
}
