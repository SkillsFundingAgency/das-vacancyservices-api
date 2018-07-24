using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using NUnit.Framework.Internal;
using ApiTypes = Esfa.Vacancy.Api.Types;

namespace Esfa.Vacancy.UnitTests.GetApprenticeshipVacancy.Api.Mappings.GivenRecruitApprenticeshipMapper
{
    [TestFixture]
    public class WhenMappingFromLiveVacancy : RecruitApprenticeshipMapperBase
    {
        private ApiTypes.ApprenticeshipVacancy _mappedVacancy;

        [SetUp]
        public void Setup()
        {
            Initialize();

            var sut = GetRecruitApprecticeshipMapper();

            _mappedVacancy = sut.MapFromRecruitVacancy(LiveVacancy).Result;
        }

        [Test]
        public void ThenMapVacancyReference()
        {
            _mappedVacancy.VacancyReference.Should().Be(LiveVacancy.VacancyReference);
        }

        [Test]
        public void ThenMapTitle()
        {
            _mappedVacancy.Title.Should().Be(LiveVacancy.Title);
        }

        [Test]
        public void ThenMapShortDescription()
        {
            _mappedVacancy.ShortDescription.Should().Be(LiveVacancy.ShortDescription);
        }

        [Test]
        public void ThenMapDescription()
        {
            _mappedVacancy.Description.Should().Be(LiveVacancy.Description);
        }

        [Test]
        public void ThenMapWorkingWeek()
        {
            _mappedVacancy.WorkingWeek.Should().Be(LiveVacancy.Wage.WorkingWeekDescription);
        }

        [Test]
        public void ThenMapWageAdditionalInformation()
        {
            _mappedVacancy.WageAdditionalInformation.Should().Be(LiveVacancy.Wage.WageAdditionalInformation);
        }

        [Test]
        public void ThenMapWageHoursPerWeek()
        {
            _mappedVacancy.HoursPerWeek.Should().Be(LiveVacancy.Wage.WeeklyHours);
        }

        [Test]
        public void ThenMapExpectedStartDate()
        {
            _mappedVacancy.ExpectedStartDate.Should().Be(LiveVacancy.StartDate);
        }

        [Test]
        public void ThenMapPostedDate()
        {
            _mappedVacancy.PostedDate.Should().Be(LiveVacancy.LiveDate);
        }

        [Test]
        public void ThenMapApplicationClosingDate()
        {
            _mappedVacancy.ApplicationClosingDate.Should().Be(LiveVacancy.ClosingDate);
        }

        [Test]
        public void ThenMapNumberOfPositions()
        {
            _mappedVacancy.NumberOfPositions.Should().Be(LiveVacancy.NumberOfPositions);
        }

        [Test]
        public void ThenMapEmployerName()
        {
            _mappedVacancy.EmployerName.Should().Be(LiveVacancy.EmployerName);
        }

        [Test]
        public void ThenMapEmployerDescription()
        {
            _mappedVacancy.EmployerDescription.Should().Be(LiveVacancy.EmployerDescription);
        }

        [Test]
        public void ThenMapEmployerWebsite()
        {
            _mappedVacancy.EmployerWebsite.Should().Be(LiveVacancy.EmployerWebsiteUrl);
        }

        [Test]
        public void ThenMapContactName()
        {
            _mappedVacancy.ContactName.Should().Be(LiveVacancy.EmployerContactName);
        }

        [Test]
        public void ThenMapContactEmail()
        {
            _mappedVacancy.ContactEmail.Should().Be(LiveVacancy.EmployerContactEmail);
        }

        [Test]
        public void ThenMapContactNumber()
        {
            _mappedVacancy.ContactNumber.Should().Be(LiveVacancy.EmployerContactPhone);
        }

        [Test]
        public void ThenMapTrainingToBeProvided()
        {
            _mappedVacancy.TrainingToBeProvided.Should().Be(LiveVacancy.TrainingDescription);
        }

        [Test]
        public void ThenMapPersonalQualities()
        {
            _mappedVacancy.PersonalQualities.Should().BeNull();
        }

        [Test]
        public void ThenMapApplicationInstructions()
        {
            _mappedVacancy.ApplicationInstructions.Should().Be(LiveVacancy.ApplicationInstructions);
        }

        [Test]
        public void ThenMapApplicationUrl()
        {
            _mappedVacancy.ApplicationUrl.Should().Be(LiveVacancy.ApplicationUrl);
        }

        [Test]
        public void ThenMapFutureProspects()
        {
            _mappedVacancy.FutureProspects.Should().Be(LiveVacancy.OutcomeDescription);
        }

        [Test]
        public void ThenMapThingsToConsider()
        {
            _mappedVacancy.ThingsToConsider.Should().Be(LiveVacancy.ThingsToConsider);
        }

        [Test]
        public void ThenMapIsNationwide()
        {
            _mappedVacancy.IsNationwide.Should().BeFalse();
        }

        [Test]
        public void ThenMapSupplementaryQuestion1()
        {
            _mappedVacancy.SupplementaryQuestion1.Should().BeNull();
        }

        [Test]
        public void ThenMapSupplementaryQuestion2()
        {
            _mappedVacancy.SupplementaryQuestion2.Should().BeNull();
        }

        [Test]
        public void ThenMapTrainingProviderName()
        {
            _mappedVacancy.TrainingProviderName.Should().Be(LiveVacancy.TrainingProvider.Name);
        }

        [Test]
        public void ThenMapTrainingProviderUkprn()
        {
            _mappedVacancy.TrainingProviderUkprn.Should().Be(LiveVacancy.TrainingProvider.Ukprn.ToString());
        }

        [Test]
        public void ThenMapTrainingProviderSite()
        {
            _mappedVacancy.TrainingProviderSite.Should().BeNull();
        }

        [Test]
        public void ThenMapIsEmployerDisabilityConfident()
        {
            _mappedVacancy.IsEmployerDisabilityConfident.Should().BeFalse();
        }

        [Test]
        public void ThenMapApprenticeshipLevel()
        {
            _mappedVacancy.ApprenticeshipLevel.Should().Be(LiveVacancy.ProgrammeLevel);
        }

        [Test]
        public void ThenMapLocation()
        {
            _mappedVacancy.Location.AddressLine1.Should().Be(LiveVacancy.EmployerLocation.AddressLine1);
            _mappedVacancy.Location.AddressLine2.Should().Be(LiveVacancy.EmployerLocation.AddressLine2);
            _mappedVacancy.Location.AddressLine3.Should().Be(LiveVacancy.EmployerLocation.AddressLine3);
            _mappedVacancy.Location.AddressLine4.Should().Be(LiveVacancy.EmployerLocation.AddressLine4);
            _mappedVacancy.Location.AddressLine5.Should().BeNull();
            _mappedVacancy.Location.Town.Should().BeNull();
            _mappedVacancy.Location.PostCode.Should().Be(LiveVacancy.EmployerLocation.Postcode);
            _mappedVacancy.Location.GeoPoint.Latitude.Should().Be((decimal)LiveVacancy.EmployerLocation.Latitude);
            _mappedVacancy.Location.GeoPoint.Longitude.Should().Be((decimal)LiveVacancy.EmployerLocation.Longitude);
        }

        [Test]
        public void ThenMapSkillsRequired()
        {
            _mappedVacancy.SkillsRequired.Should().Be(string.Join(",", LiveVacancy.Skills));
        }

        [Test]
        public void ThenMapQualificationsRequired()
        {
            var formattedDescriptions = LiveVacancy.Qualifications.Select(q => $"{q.QualificationType} {q.Subject} (Grade {q.Grade}) {q.Weighting}");
            var output = string.Join(",", formattedDescriptions);

            _mappedVacancy.QualificationsRequired.Should().Be(output);
        }
    }
}
