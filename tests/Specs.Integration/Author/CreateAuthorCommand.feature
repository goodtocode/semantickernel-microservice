@createAuthorCommand
Feature: Create Author Command
As a owner
When I want to save a new Author
Then I should see the Author created with the initial response

Scenario: Create Author
	Given I have a def "<def>"
	And I have a name "<name>"
	And I have a Author key "<key>"
	And The Author exists "<exists>"
	When I create a author
	Then I see the Author created with the initial response "<response>"
	And if the response has validation issues I see the "<responseErrors>" in the response
 
Examples:
	| def                        | response   | responseErrors | key                                  | exists | name     |
	| success                    | Success    |                | 00000000-0000-0000-0000-000000000000 | false  | John Doe |
	| bad request: empty message | BadRequest | Name           | 00000000-0000-0000-0000-000000000000 | false  |          |
	| already exists             | Error      |                | 038d8e7f-f18f-4a8e-8b3c-3b6a6889fed9 | true   | Jane Doe |