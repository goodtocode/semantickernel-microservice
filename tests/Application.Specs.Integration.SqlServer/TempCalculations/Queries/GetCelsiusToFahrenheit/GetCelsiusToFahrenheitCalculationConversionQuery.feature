
@getCelsiusToFahrenheitCalculationConversionQuery
Feature: Get Celsius To Fahrenheit Calculation Conversion Query
As a weather forecasts user
I am able to get a Celsius To Fahrenheit Calculation Conversion

Scenario: Get Celsius To Fahrenheit Calculation Conversion Query
	Given I have an Celsius value to convert
	When  I get a Celsius To Fahrenheit Calculation Conversion
	Then  I receive a successful response
	And   The response is the correct calculation
