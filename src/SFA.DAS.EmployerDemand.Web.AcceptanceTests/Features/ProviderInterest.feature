Feature: ProviderInterest
	As an Provider
	I want to be able to see Employer demands
	So that I can contact employers about training opportunities 


@WireMockServer
Scenario: Basic content
When I navigate to the following url: /10000001/find-apprenticeship-opportunities
Then an http status code of 200 is returned
And the page content includes the following: Find employers that need a training provider

@MockApiClient
Scenario: Filter Demands
When I navigate to the following url: /10000001/find-apprenticeship-opportunities?SelectedCourseId=50  
Then the Api is called with the course id 50

