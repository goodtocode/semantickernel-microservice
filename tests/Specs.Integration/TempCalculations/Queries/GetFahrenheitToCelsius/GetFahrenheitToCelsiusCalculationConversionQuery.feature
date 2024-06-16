
@getFahrenheitToCelsiusCalculationConversionQuery
Feature: Get Fahrenheit To Celsius Calculation Conversion Query
As a weather forecasts user
I am able to get a Fahrenheit To Celsius Calculation Conversion

Scenario: Get Fahrenheit To Celsius Calculation Conversion
	Given I have an Fahrenheit value to convert
	When  I get a Fahrenheit To Celsius Calculation Conversion
	Then  I receive a successful response
	And   The response contains is the correct calculation
