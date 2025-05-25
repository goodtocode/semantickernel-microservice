@getChatSessionsQuery
Feature: Get Chat Sessions Query
As a session owner
When I query chat sessions optionally by date range
I get all sessions that fit the date range

Scenario: Get chat sessions
	Given I have a definition "<def>"
	And Chat Sessions exist "<exist>"
	And chat sessions within the date range exists "<chatSessionsResultExists>"
	And I have a start date "<startDate>"
	And I have a end date "<endDate>"	
	When I get the chat sessions
	Then The response is "<response>"
	And If the response has validation issues I see the "<responseErrors>" in the response
	And The response has a collection of chat sessions
	And Each chat session has a Key
	And Each chat session has a Date greater than start date
	And Each chat session has a Date less than end date

Examples:
	| def                      | response | responseErrors | startDate            | endDate              | exist | chatSessionsResultExists |
	| success no date range    | Success  |                |                      |                      | true  | true                     |
	| success with date range  | Success  |                | 2024-06-01T11:21:00Z | 2034-06-03T11:21:00Z | true  | true                     |
	| success filtered results | Success  |                | 2024-06-01T11:21:00Z | 2034-06-03T11:21:00Z | true  | false                    |
	| success empty results    | Success  |                |                      |                      | false | false                    |