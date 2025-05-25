@getTextPromptsQuery
Feature: Get Text Prompts Query
As a session owner
When I query text prompt optionally by date range
I get all sessions that fit the date range

Scenario: Get text prompts
	Given I have a definition "<def>"
	And Text Prompt exist "<exist>"
	And text prompt within the date range exists "<textPromptsResultExists>"
	And I have a start date "<startDate>"
	And I have a end date "<endDate>"	
	When I get the text prompt
	Then The response is "<response>"
	And If the response has validation issues I see the "<responseErrors>" in the response
	And The response has a collection of text prompt
	And Each text prompt has a Key
	And Each text prompt has a Date greater than start date
	And Each text prompt has a Date less than end date

Examples:
	| def                      | response | responseErrors | startDate            | endDate              | exist | textPromptsResultExists |
	| success no date range    | Success  |                |                      |                      | true  | true                     |
	| success with date range  | Success  |                | 2024-06-01T11:21:00Z | 2034-06-03T11:21:00Z | true  | true                     |
	| success filtered results | Success  |                | 2024-06-01T11:21:00Z | 2034-06-03T11:21:00Z | true  | false                    |
	| success empty results    | Success  |                |                      |                      | false | false                    |