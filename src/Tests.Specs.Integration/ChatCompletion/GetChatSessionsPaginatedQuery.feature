@getChatSessionsPaginatedQuery
Feature: Get Chat Sessions Paginated Query
As an owner of chat sessions
When I get chat sessions all or by date range
I can get a paginated collection of chat sessions

Scenario: Get chat sessions paginated
	Given I have a definition "<def>"
	And Chat Sessions exist "<exist>"
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
	| def                          | response   | responseErrors | startDate            | endDate              | exist | chatSessionsResultExists | pageNumber | pageSize |
	| success no date range        | Success    |                |                      |                      | true  | true                     | 1          | 10       |
	| success with date range      | Success    |                | 2024-06-01T11:21:00Z | 2034-06-03T11:21:00Z | true  | true                     | 1          | 10       |
	| success empty results        | Success    |                |                      |                      | false | false                    | 1          | 10       |
	| bad request page number zero | BadRequest | PageNumber     |                      |                      | false | false                    | 0          | 10       |
	| bad request page size zero   | BadRequest | PageSize       |                      |                      | false | false                    | 1          | 0       |