@getAllForecastsQuery
Feature: Get All Forecasts
As a weather forecasts user
I can get all forecasts

Scenario: Get all forecasts
	Given I have a definition "<def>"
	And Forecasts Exist "<forecastsExist>"
	And I have a zipcode filter "<zipcodeFilter>"
	And A forecast with zipcodeFilter exists "<forecastsWithZipcodeFilterExist>"
	When I get all forecasts
	Then The response is "<response>"
	And The response has a collection of forecasts
	And Each forecast has a Key
	And Each forecast has a Date
	And Each forecast has a TemperatureC
	And Each forecast has a TemperatureF
	And Each forecast has a Summary
	And Each forecast has a collection of Zipcodes

Examples:
	| def                      | response | zipcodeFilter | forecastsExist | forecastsWithZipcodeFilterExist |
	| success                  | Success  |               | true           | true                            |
	| success filtered results | Success  | 92602         | true           | false                           |
	| success empty results    | Success  | 92602         | false          | false                           |