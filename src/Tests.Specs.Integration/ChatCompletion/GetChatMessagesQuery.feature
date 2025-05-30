@getChatMessagesQuery
Feature: Get Chat Messages Query
As a message owner
When I query Chat Messages optionally by date range
I get all messages that fit the date range

Scenario: Get Chat Messages
	Given I have a definition "<def>"
	And Chat Messages exist "<exist>"
	And Chat Messages within the date range exists "<ChatMessagesResultExists>"
	And I have a start date "<startDate>"
	And I have a end date "<endDate>"	
	When I get the Chat Messages
	Then The response is "<response>"
	And If the response has validation issues I see the "<responseErrors>" in the response
	And The response has a collection of Chat Messages
	And Each Chat Message has a Key
	And Each Chat Message has a Date greater than start date
	And Each Chat Message has a Date less than end date

Examples:
	| def                      | response | responseErrors | startDate            | endDate              | exist | ChatMessagesResultExists |
	| success no date range    | Success  |                |                      |                      | true  | true                     |
	| success with date range  | Success  |                | 2024-06-01T11:21:00Z | 2034-06-03T11:21:00Z | true  | true                     |
	| success filtered results | Success  |                | 2024-06-01T11:21:00Z | 2034-06-03T11:21:00Z | true  | false                    |
	| success empty results    | Success  |                |                      |                      | false | false                    |