@getPaginatedSemanticKernelMicroserviceQuery
Feature: Get Weather Forecasts Paginated Query
As a weather forecasts user
I can get a paginated collection of forecasts

Scenario: Get paginated weather forecasts
	Given I have a definition "<def>"
	And A collection of Weather Forecasts Exist "<forecastsExist>"
	And I have a page number "<pageNumber>"
	And I have a page size "<pageSize>"
	When I get paginated weather forecasts
	Then The response is "<response>"
	And The response has a collection of weather forecasts
	And The response has a Page Number
	And The response has a Total Pages
	And The response has a Total Count
	And The response has a collection of items
	And Each weather forecast has a Key
	And Each weather forecast has a Date
	And Each weather forecast has a TemperatureC
	And Each weather forecast has a TemperatureF
	And Each weather forecast has a Summary
	And Each weather forecast has a Zipcodes
	And If the response has validation issues I see the "<responseErrors>" in the response

Examples:
	| def                          | response   | responseErrors | pageNumber | pageSize | forecastsExist |
	| success                      | Success    |                | 1          | 10       | true           |
	| success filtered results     | Success    |                | 1          | 10       | true           |
	| success empty results        | Success    |                | 1          | 10       | false          |
	| bad request page number zero | BadRequest | PageNumber     | 0          | 10       | true           |
	| bad request page size zero   | BadRequest | PageSize       | 1          | 0        | true           |