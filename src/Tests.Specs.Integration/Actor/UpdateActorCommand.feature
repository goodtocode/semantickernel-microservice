@updateAuthorCommand
Feature: Update Actor Command
As a Actor owner
When I edit a Actor
I am able to change or add to the Actor

Scenario: Update Actor
	Given I have a def "<def>"
	And I have a Actor id "<id>"
	And the Actor exists "<exists>"
	When I update the Actor
	Then The response is "<response>"
	And If the response has validation issues I see the "<responseErrors>" in the response
 
Examples:
	| def                   | response   | responseErrors | id                                   | exists |
	| success               | Success    |                | 038d8e7f-f18f-4a8e-8b3c-3b6a6889fed9 | true   |
	| not found             | NotFound   |                | 038d8e7f-f18f-4a8e-8b3c-3b6a6889fed9 | false  |
