@removeForecastCommand
Feature: Remove Forecast Command
As a weather forecasts user
I can remove a forecast

Scenario: Remove Forecast
	Given I have a def "<def>"
	And I have a forecast key"<key>"
	And The forecast exists "<forecastExists>"
	When I remove the forecast
	Then The response is "<response>"
	And If the response has validation issues I see the "<responseErrors>" in the response
 
Examples:
	| def                    | response   | responseErrors | key                                  | forecastExists |
	| success                | Success    |                | 038d8e7f-f18f-4a8e-8b3c-3b6a6889fed9 | true           |
	| not found              | NotFound   |                | 038d8e7f-f18f-4a8e-8b3c-3b6a6889fed9 | false          |
	| bad request: empty key | BadRequest | Key            | 00000000-0000-0000-0000-000000000000 | false          |