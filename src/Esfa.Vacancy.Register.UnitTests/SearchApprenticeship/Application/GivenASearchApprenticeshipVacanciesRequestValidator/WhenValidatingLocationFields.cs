﻿using System;
using System.Collections.Generic;
using Esfa.Vacancy.Register.Application.Queries.SearchApprenticeshipVacancies;
using Esfa.Vacancy.Register.Domain.Entities;
using Esfa.Vacancy.Register.Domain.Validation;
using FluentAssertions;
using NUnit.Framework;

namespace Esfa.Vacancy.Register.UnitTests.SearchApprenticeship.Application.GivenASearchApprenticeshipVacanciesRequestValidator
{
    [TestFixture]
    public class WhenValidatingLocationFields : GivenSearchApprenticeshipVacanciesRequestValidatorBase
    {
        [Test]
        public void AndAllThreeFieldsArePresentAndEachFieldIsValid_ThenRequestIsValid()
        {
            var result = Validator.Validate(new SearchApprenticeshipVacanciesRequest
            {
                StandardLarsCodes = ValidStandardCodes,
                Latitude = 52.399085,
                Longitude = -1.506115,
                DistanceInMiles = 342
            });

            result.IsValid.Should().BeTrue();
        }

        private static List<TestCaseData> FailingTestCases => new List<TestCaseData>
        {
            new TestCaseData(new SearchApprenticeshipVacanciesRequest
                    {
                        StandardLarsCodes = ValidStandardCodes,
                        Latitude = 52.399085,
                        Longitude = -1.506115
                    },
                ErrorMessages.SearchApprenticeships.GetTrainingCodeShouldBeNumberErrorMessage(TrainingType.Framework, "e"),
                ErrorCodes.SearchApprenticeships.FrameworkCodeNotInt32)
                .SetName("And no distance then invalid"),
            new TestCaseData(new SearchApprenticeshipVacanciesRequest
                    {
                        StandardLarsCodes = ValidStandardCodes,
                        Longitude = -1.506115,
                        DistanceInMiles = 342
                    },
                ErrorMessages.SearchApprenticeships.GetTrainingCodeShouldBeNumberErrorMessage(TrainingType.Framework, "1.1"),
                ErrorCodes.SearchApprenticeships.FrameworkCodeNotInt32)
                .SetName("And no latitude then invalid"),
            new TestCaseData(new SearchApprenticeshipVacanciesRequest
                    {
                        StandardLarsCodes = ValidStandardCodes,
                        Latitude = 52.399085,
                        DistanceInMiles = 342
                    },
                ErrorMessages.SearchApprenticeships.GetTrainingCodeShouldBeNumberErrorMessage(TrainingType.Framework, "2 0"),
                ErrorCodes.SearchApprenticeships.FrameworkCodeNotInt32)
                .SetName("And no longitude then invalid")
        };

        [TestCaseSource(nameof(FailingTestCases))]
        public void ValidateMissingFields(SearchApprenticeshipVacanciesRequest request, string expectedErrorMessage, string expectedErrorCode)
        {
            var result = Validator.Validate(request);

            result.IsValid.Should().Be(false);
            result.Errors.Count.Should().Be(1);
            //result.Errors.First().ErrorMessage.Should().Be(expectedErrorMessage);
            //result.Errors.First().ErrorCode.Should().Be(expectedErrorCode);
        }
    }
}