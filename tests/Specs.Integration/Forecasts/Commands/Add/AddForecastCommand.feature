@addForecastCommand
Feature: Add Forecast Command
As a weather forecasts user
I can add a forecast 

Scenario: Add Forecast
	Given I have a def "<def>"
	And I have a forecast key "<key>"
	And The forecast exists "<forecastExists>"
	And I have a forecast date "<date>"
	And I have a forecast TemperatureF "<temperatureF>"
	And I have a collection of Zipcodes "<zipcodes>"
	When I add the forecast
	Then The response is "<response>"
	And If the response has validation issues I see the "<responseErrors>" in the response
 
Examples:
	| def                             | response   | responseErrors | key                                  | forecastExists | date     | temperatureF | zipcodes     |
	| success                         | Success    |                | 038d8e7f-f18f-4a8e-8b3c-3b6a6889fed9 | false          | 5/2/2023 | 75           | 92222, 93333 |
	| bad request: empty key          | BadRequest | Key            |                                      | false          | 5/2/2023 | 75           |              |
	| bad request: empty TemperatureF | BadRequest | TemperatureF   | 038d8e7f-f18f-4a8e-8b3c-3b6a6889fed9 | false          | 5/3/2023 |              | 92222        |
	| bad request: empty date         | BadRequest | Date           | 038d8e7f-f18f-4a8e-8b3c-3b6a6889fed9 | false          |          | 75           | 92222, 93333 |
	| already exists                  | BadRequest | Key            | 038d8e7f-f18f-4a8e-8b3c-3b6a6889fed9 | true           | 5/3/2023 | 75           | 92222, 93333 |