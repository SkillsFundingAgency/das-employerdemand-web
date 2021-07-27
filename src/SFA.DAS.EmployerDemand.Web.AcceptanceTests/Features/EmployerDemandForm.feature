Feature: EmployerDemandForm
	As an Employer
	I want to submit my data
	So that I can be contacted by providers who offer the course I want 


@WireMockServer
Scenario: Basic content
When I navigate to the following url: /registerdemand/course/34/enter-apprenticeship-details
Then an http status code of 200 is returned
And the page content includes the following: Get help with finding a training provider


@WireMockServer
Scenario: Location Validation failure for invalid location
When I post to the following url: /registerdemand/course/34/enter-apprenticeship-details
| Field | Value |
|TrainingCourseId         | 34                      |
| Location                 | Cov	 				 |
| NumberOfApprenticesKnown | true                    |
| NumberOfApprentices      | 23                      |
| OrganisationName         | Bob the builder         |
| ContactEmailAddress      | bob@bobthebuilder.com   |
Then an http status code of 200 is returned
And the page content includes the following: Enter a real town, city or postcode

@WireMockServer
Scenario: Valid Employer Demand Submittied
Given I post to the following url: /registerdemand/course/34/enter-apprenticeship-details
| Field                    | Value                 |
| TrainingCourseId         | 34                    |
| Location                 | Camden, Camden        |
| NumberOfApprenticesKnown | true                  |
| NumberOfApprentices      | 23                    |
| OrganisationName         | Bob the builder       |
| ContactEmailAddress      | bob@bobthebuilder.com |
Then an http status code of 302 is returned
When I navigate to location header url
Then an http status code of 200 is returned
And the page content includes the following: Check your answers
When I post the viewed page
Then an http status code of 302 is returned
When I navigate to location header url
Then an http status code of 200 is returned
And the page content includes the following: Click on the link in the email to confirm the address
When I validate the demand 
Then an http status code of 200 is returned
And the page content includes the following:  We’ve shared this interest with training providers

@WireMockServer
Scenario: Stop Sharing Interest 
Given I post to the following url: /registerdemand/course/34/enter-apprenticeship-details
| Field                    | Value                 |
| TrainingCourseId         | 34                    |
| Location                 | Camden, Camden        |
| NumberOfApprenticesKnown | true                  |
| NumberOfApprentices      | 23                    |
| OrganisationName         | Bob the builder       |
| ContactEmailAddress      | bob@bobthebuilder.com |
Then an http status code of 302 is returned
When I navigate to location header url
Then an http status code of 200 is returned
And the page content includes the following: Check your answers
When I post the viewed page
Then an http status code of 302 is returned
When I navigate to location header url
Then an http status code of 200 is returned
And the page content includes the following: Click on the link in the email to confirm the address
When I stop sharing the demand 
Then an http status code of 200 is returned
And the page content includes the following:  stopped sharing your interest in apprenticeship training with training providers

@WireMockServer
Scenario: Restart Sharing Interest 
Given I post to the following url: /registerdemand/course/34/enter-apprenticeship-details
| Field                    | Value                 |
| TrainingCourseId         | 34                    |
| Location                 | Camden, Camden        |
| NumberOfApprenticesKnown | true                  |
| NumberOfApprentices      | 23                    |
| OrganisationName         | Bob the builder       |
| ContactEmailAddress      | bob@bobthebuilder.com |
Then an http status code of 302 is returned
When I navigate to location header url
Then an http status code of 200 is returned
And the page content includes the following: Check your answers
When I post the viewed page
Then an http status code of 302 is returned
When I navigate to location header url
Then an http status code of 200 is returned
And the page content includes the following: Click on the link in the email to confirm the address
When I stop sharing the demand 
Then an http status code of 200 is returned
And the page content includes the following:  stopped sharing your interest in apprenticeship training with training providers
When I restart sharing demand
Then an http status code of 302 is returned
And the page content includes the following: Check your answers
