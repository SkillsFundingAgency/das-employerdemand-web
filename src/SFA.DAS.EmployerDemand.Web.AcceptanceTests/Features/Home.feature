
Feature: Home
	As a AEDB user
	I want a clear home page
	So that it is clear what actions I can take

@WireMockServer
	Scenario: Navigate to start page
	When I navigate to the following url:/registerdemand/course/34/share-interest
	Then an http status code of 200 is returned
	And the page content includes the following: Share your interest with training providers