﻿using System;
using System.Collections.Generic;
using Esfa.Vacancy.Application.Queries.SearchApprenticeshipVacancies;
using Esfa.Vacancy.Domain.Validation;
using FluentAssertions;
using FluentValidation.Results;
using NUnit.Framework;

namespace Esfa.Vacancy.UnitTests.SearchApprenticeship.Application.GivenASearchApprenticeshipVacanciesRequestValidator
{
    [TestFixture]
    public class WhenValidatingPageSize : GivenSearchApprenticeshipVacanciesRequestValidatorBase
    {
        [TestCaseSource(nameof(TestCases))]
        public void AndCheckingIsValid(SearchApprenticeshipVacanciesRequest searchRequest, ValidationResult expectedResult)
        {
            var actualResult = Validator.Validate(searchRequest);

            actualResult.IsValid.Should().Be(expectedResult.IsValid);
        }

        [TestCaseSource(nameof(TestCases))]
        public void AndCheckingErrorMessages(SearchApprenticeshipVacanciesRequest searchRequest, ValidationResult expectedResult)
        {
            var actualResult = Validator.Validate(searchRequest);

            actualResult.Errors.ShouldAllBeEquivalentTo(expectedResult.Errors,
                options => options.Including(failure => failure.ErrorMessage));
        }

        [TestCaseSource(nameof(TestCases))]
        public void AndCheckingErrorCodes(SearchApprenticeshipVacanciesRequest searchRequest, ValidationResult expectedResult)
        {
            var actualResult = Validator.Validate(searchRequest);

            actualResult.Errors.ShouldAllBeEquivalentTo(expectedResult.Errors,
                options => options.Including(failure => failure.ErrorCode));
        }

        private static List<TestCaseData> TestCases => new List<TestCaseData>
        {
            new TestCaseData(new SearchApprenticeshipVacanciesRequest
                {
                    NationwideOnly = true,
                }, new ValidationResult())
                .SetName("Then default is valid"),
            new TestCaseData(new SearchApprenticeshipVacanciesRequest
                {
                    NationwideOnly = true,
                    PageSize = 0
                }, new ValidationResult
                {
                    Errors = { new ValidationFailure("PageSize", "'Page Size' must be between 1 and 250. You entered 0.")
                    {
                        ErrorCode = ErrorCodes.SearchApprenticeships.PageSize
                    }}
                })
                .SetName("Then less than 1 is invalid"),
            new TestCaseData(new SearchApprenticeshipVacanciesRequest
                {
                    NationwideOnly = true,
                    PageSize = 1
                }, new ValidationResult())
                .SetName("Then 1 is valid"),
            new TestCaseData(new SearchApprenticeshipVacanciesRequest
                {
                    NationwideOnly = true,
                    PageSize = new Random().Next(1, 250)
                }, new ValidationResult())
                .SetName("Then between 1 and 250 is valid"),
            new TestCaseData(new SearchApprenticeshipVacanciesRequest
                {
                    NationwideOnly = true,
                    PageSize = 250
                }, new ValidationResult())
                .SetName("Then 250 is valid"),
            new TestCaseData(new SearchApprenticeshipVacanciesRequest
                {
                    NationwideOnly = true,
                    PageSize = 251
                }, new ValidationResult
                {
                    Errors = { new ValidationFailure("PageSize", "'Page Size' must be between 1 and 250. You entered 251.")
                    {
                        ErrorCode = ErrorCodes.SearchApprenticeships.PageSize
                    }}
                })
                .SetName("Then greater than 250 is invalid")
        };
    }
}
