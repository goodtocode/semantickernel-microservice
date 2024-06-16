@patchForecastCommand
Feature: Patch Forecast Command
As a weather forecasts user
I can patch a forecast 

Scenario: Patch Forecast
	Given I have a def "<def>"
	And I have a forecast key "<key>"
	And the forecast exists "<forecastExists>"
	And I have a forecast date "<date>"
	And I have a forecast TemperatureF "<temperatureF>"
	And I have a collection of Zipcodes "<zipcodes>"
	When I patch the forecast
	Then The response is "<response>"
	And If the response has validation issues I see the "<responseErrors>" in the response
 
Examples:
	| def                            | response   | responseErrors | key                                  | forecastExists | date     | temperatureF | zipcodes     |
	| success : patch all            | Success    |                | 038d8e7f-f18f-4a8e-8b3c-3b6a6889fed9 | true           | 5/2/2023 | 75           | 92222, 93333 |
	| success : patch date           | Success    |                | 038d8e7f-f18f-4a8e-8b3c-3b6a6889fed9 | true           | 5/2/2023 | 75           | 92222, 93333 |
	| success : patch temperatureC   | Success    |                | 038d8e7f-f18f-4a8e-8b3c-3b6a6889fed9 | true           | 5/2/2023 |              |              |
	| success : patch zipcodes       | Success    |                | 038d8e7f-f18f-4a8e-8b3c-3b6a6889fed9 | true           |          |              | 92222, 93333 |
	| bad request: empty key         | BadRequest | Key            |                                      | false          | 5/2/2023 | 75           | 92222, 93333 |
	| not found : patch date         | NotFound   |                | 038d8e7f-f18f-4a8e-8b3c-3b6a6889fed9 | false          | 5/2/2023 | 75           |              |
	| not found : patch temperatureC | NotFound   |                | 038d8e7f-f18f-4a8e-8b3c-3b6a6889fed9 | false          |          | 75           |              |
	| not found : patch zipcodes     | NotFound   |                | 038d8e7f-f18f-4a8e-8b3c-3b6a6889fed9 | false          |          |              | 92222, 93333 |