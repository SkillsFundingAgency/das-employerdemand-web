﻿Feature: EmployerDemandForm
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
| Field                    | Value                   |
| TrainingCourseId         | 34                      |
| Location                 | Cov	 				 |
| NumberOfApprenticesKnown | true                    |
| NumberOfApprentices      | 23                      |
| OrganisationName         | Bob the builder         |
| ContactEmailAddress      | bob@bobthebuilder.com   |
Then an http status code of 200 is returned
And the page content includes the following: Enter a real town, city or postcode

