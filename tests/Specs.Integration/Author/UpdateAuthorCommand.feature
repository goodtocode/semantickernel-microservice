@updateAuthorCommand
Feature: Update Author Command
As a Author owner
When I edit a Author
I am able to change or add to the Author

Scenario: Update Author
	Given I have a def "<def>"
	And I have a Author key "<key>"
	And the Author exists "<exists>"
	When I update the Author
	Then The response is "<response>"
	And If the response has validation issues I see the "<responseErrors>" in the response
 
Examples:
	| def                             | response   | responseErrors | key                                  | exists |
	| success                         | Success    |                | 038d8e7f-f18f-4a8e-8b3c-3b6a6889fed9 | true   |
	| not found                       | NotFound   |                | 038d8e7f-f18f-4a8e-8b3c-3b6a6889fed9 | false  |
	| bad request: empty key          | BadRequest | Key            | 00000000-0000-0000-0000-000000000000 | false  |