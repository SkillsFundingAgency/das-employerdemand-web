Feature: Provider register interest
	As a provider
	I want to register interest to employer demand
	So that employers know I want to provide their course

@WireMockServer
Scenario: Provider registers interest
Given I post to the following url: /10000001/find-apprenticeship-opportunities/14
| Field                    | Value                 |
| EmployerDemands         | 7a691536-ecb7-4a7d-9639-8768a555e099                    |
| CourseLevel | 2 |
| CourseTitle | Business and administration |
| CourseSector | Sector |
Then an http status code of 302 is returned
When I navigate to location header url