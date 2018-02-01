#Feature: ApprenticeshipDetail
#	In order to avoid silly mistakes
#	As a math idiot
#	I want to be told the sum of two numbers
#
#@mytag
#Scenario: Add two numbers
#	Given I have entered 50 into the calculator
#	And I have entered 70 into the calculator
#	When I press add
#	Then the result should be 120 on the screen


Feature: RA388
	In order to attract more candidates
	As a vacancy manager
	I want to be able to increase the wage of a vacancy

@GetVacancyDetailsByVacancyReferenceNumber
Scenario: Get vacancy details for Live Apprenticeship Vacancy
	When I request the vacancy details for the apprenticeship with reference number: 87654321
	Then The response status is: OK
	And The response body contains a field called Vacancy Reference Number