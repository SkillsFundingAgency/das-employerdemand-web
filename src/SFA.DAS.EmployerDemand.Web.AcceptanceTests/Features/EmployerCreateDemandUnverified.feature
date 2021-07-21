Feature: EmployerCreateDemandUnverified
	As ESFA
	I want to tell the employer to verify their email
	so that I know their contact details are correct

@WireMockServer
Scenario: Basic content
When I post to the following url: 
Then an http status code of 200 is returned
And the page content includes the following: Click the link we've sent to