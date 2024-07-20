@getAuthorChatSessionsQuery
Feature: Get Author Chat Sessions Query
As an author
When I query chat sessions optionally by date range
I get all sessions that fit the date range

Scenario: Get author chat sessions
	Given I have a definition "<def>"
	And Chat Sessions exist "<exist>"
	And I have a Author id "<id>"
	And I have a start date "<startDate>"
	And I have a end date "<endDate>"
	And chat sessions within the date range exists "<chatSessionsResultExists>"
	When I get the chat sessions
	Then The response is "<response>"
	And If the response has validation issues I see the "<responseErrors>" in the response
	And The response has a collection of chat sessions
	And Each chat session has a Key
	And Each chat session has a Date greater than start date
	And Each chat session has a Date less than end date

Examples:
	| def                      | response | responseErrors | id                                   | startDate            | endDate              | exist | chatSessionsResultExists |
	| success no date range    | Success  |                | 038d8e7f-f18f-4a8e-8b3c-3b6a6889fed9 |                      |                      | true  | true                     |
	| success with date range  | Success  |                | 038d8e7f-f18f-4a8e-8b3c-3b6a6889fed9 | 2024-06-01T11:21:00Z | 2024-06-03T11:21:00Z | true  | true                     |
	| success filtered results | Success  |                | 038d8e7f-f18f-4a8e-8b3c-3b6a6889fed9 | 2024-06-01T11:21:00Z | 2024-06-03T11:21:00Z | true  | false                    |
	| success empty results    | Success  |                | 038d8e7f-f18f-4a8e-8b3c-3b6a6889fed9 |                      |                      | false | false                    |