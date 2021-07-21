Feature: EmployerCreateDemandUnverified
	As ESFA
	I want to tell the employer to verify their email
	so that I know their contact details are correct

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
