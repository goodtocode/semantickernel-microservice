@getTextAudiosQuery
Feature: Get Text Audios Query
As a session owner
When I query text audio optionally by date range
I get all sessions that fit the date range

Scenario: Get text audios
	Given I have a definition "<def>"
	And Text Audio exist "<exist>"
	And text audio within the date range exists "<textPromptsResultExists>"
	And I have a start date "<startDate>"
	And I have a end date "<endDate>"	
	When I get the text audio
	Then The response is "<response>"
	And If the response has validation issues I see the "<responseErrors>" in the response
	And The response has a collection of text audio
	And Each text audio has a Key
	And Each text audio has a Date greater than start date
	And Each text audio has a Date less than end date

Examples:
	| def                      | response | responseErrors | startDate            | endDate              | exist | textPromptsResultExists |
	| success no date range    | Success  |                |                      |                      | true  | true                     |
	| success with date range  | Success  |                | 2024-06-01T11:21:00Z | 2034-06-03T11:21:00Z | true  | true                     |
	| success filtered results | Success  |                | 2024-06-01T11:21:00Z | 2034-06-03T11:21:00Z | true  | false                    |
	| success empty results    | Success  |                |                      |                      | false | false                    |