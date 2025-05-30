@getChatMessagesPaginatedQuery
Feature: Get Chat Messages Paginated Query
As an owner of Chat Messages
When I get Chat Messages all or by date range
I can get a paginated collection of Chat Messages

Scenario: Get Chat Messages paginated
	Given I have a definition "<def>"
	And Chat Messages exist "<exist>"
	And I have a start date "<startDate>"
	And I have a end date "<endDate>"
	And Chat Messages within the date range exists "<ChatMessagesResultExists>"
	And I have a page number "<pageNumber>"
	And I have a page size "<pageSize>"
	When I get the Chat Messages paginated
	Then The response is "<response>"
	And If the response has validation issues I see the "<responseErrors>" in the response
	And The response has a collection of Chat Messages
	And Each Chat Message has a Key
	And Each Chat Message has a Date greater than start date
	And Each Chat Message has a Date less than end date
	And The response has a Page Number
	And The response has a Total Pages
	And The response has a Total Count	


Examples:
	| def                          | response   | responseErrors | startDate            | endDate              | exist | ChatMessagesResultExists | pageNumber | pageSize |
	| success no date range        | Success    |                |                      |                      | true  | true                     | 1          | 10       |
	| success with date range      | Success    |                | 2024-06-01T11:21:00Z | 2034-06-03T11:21:00Z | true  | true                     | 1          | 10       |
	| success empty results        | Success    |                |                      |                      | false | false                    | 1          | 10       |
	| bad request page number zero | BadRequest | PageNumber     |                      |                      | false | false                    | 0          | 10       |
	| bad request page size zero   | BadRequest | PageSize       |                      |                      | false | false                    | 1          | 0       |