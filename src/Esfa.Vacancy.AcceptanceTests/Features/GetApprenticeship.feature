Feature: GetApprenticeship
	In order to read an apprenticeship
	As an api user
	I want to get the details for an apprenticeship

Scenario: Get an apprenticeship
	Given the following apprenticeships
	| Title                  |
	| Apprentice good things |
	And I have permission
	When I get the apprenticeship 'Apprentice good things'
	Then the http status code should be 200


	Scenario: No permission
	Given the following apprenticeships
	| Title                  |
	| Apprentice good things |
	When I get the apprenticeship 'Apprentice good things'
	Then the http status code should be 401