@getAuthorChatSessionsPaginatedQuery
Feature: Get Actor Chat Sessions Paginated Query
As an actor of chat sessions
When I get chat sessions by actor or by date range
I can get a paginated collection of chat sessions

Scenario: Get actor chat sessions paginated
	Given I have a definition "<def>"
	And Chat Sessions exist "<exist>"
	And I have a Actor id "<id>"
	And I have a start date "<startDate>"
	And I have a end date "<endDate>"
	And chat sessions within the date range exists "<chatSessionsResultExists>"
	And I have a page number "<pageNumber>"
	And I have a page size "<pageSize>"
	When I get the chat sessions paginated
	Then The response is "<response>"
	And If the response has validation issues I see the "<responseErrors>" in the response
	And The response has a collection of chat sessions
	And Each chat session has a Key
	And Each chat session has a Date greater than start date
	And Each chat session has a Date less than end date
	And The response has a Page Number
	And The response has a Total Pages
	And The response has a Total Count	


Examples:
	| def                          | response   | responseErrors | id                                   | startDate            | endDate              | exist | chatSessionsResultExists | pageNumber | pageSize |
	| success no date range        | Success    |                | 038d8e7f-f18f-4a8e-8b3c-3b6a6889fed9 |                      |                      | true  | true                     | 1          | 10       |
	| success with date range      | Success    |                | 038d8e7f-f18f-4a8e-8b3c-3b6a6889fed9 | 2024-06-01T11:21:00Z | 2034-06-03T11:21:00Z | true  | true                     | 1          | 10       |
	| success with date range      | Success    |                | 9c5f2b35-b380-44f8-8c71-c24a43b3fe63 | 2025-07-19T11:21:00Z | 2034-06-03T11:21:00Z | true  | true                     | 1          | 10       |
	| success empty results        | Success    |                | 038d8e7f-f18f-4a8e-8b3c-3b6a6889fed9 |                      |                      | false | false                    | 1          | 10       |
	| bad request page number zero | BadRequest | PageNumber     | 038d8e7f-f18f-4a8e-8b3c-3b6a6889fed9 |                      |                      | false | false                    | 0          | 10       |
	| bad request page size zero   | BadRequest | PageSize       | 038d8e7f-f18f-4a8e-8b3c-3b6a6889fed9 |                      |                      | false | false                    | 1          | 0        |