Feature: Provider register interest
	As a provider
	I want to register interest to employer demand
	So that employers know I want to provide their course

@WireMockServer
Scenario: Provider registers interest
Given I post to the following url: /10000001/find-apprenticeship-opportunities/14
| Field                   | Value                                                   |
| id                      | afde50cd-a434-45bd-827b-4b96f3930003                    |
| EmployerDemands         | 7a691536-ecb7-4a7d-9639-8768a555e099\|1\|Camden, Camden |
| CourseLevel             | 2                                                       |
| CourseTitle             | Business and administration                             |
| CourseSector            | Sector                                                  |
| ProviderEmail           | test@test.com                                           |
| ProviderTelephoneNumber | 0123123123                                              |
Then an http status code of 302 is returned
When I navigate to location header url
Then an http status code of 200 is returned
And the page content includes the following: share these with the employers you're showing interest in
When I navigate to the following url: /10000001/find-apprenticeship-opportunities/14/confirm/afde50cd-a434-45bd-827b-4b96f3930003/review
Then an http status code of 200 is returned
And the page content includes the following: Check your answers
When I post to the following url: 10000001/find-apprenticeship-opportunities/14/shared-contact-details/afde50cd-a434-45bd-827b-4b96f3930003
  | Field | Value                                |
  | id    | afde50cd-a434-45bd-827b-4b96f3930003 |
Then an http status code of 302 is returned
When I navigate to location header url
Then an http status code of 200 is returned
And the page content includes the following: shared your contact details with employers