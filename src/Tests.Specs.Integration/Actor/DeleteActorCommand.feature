@deleteAuthorCommand
Feature: Delete Actor Command
As a Actor owner
When I select a Actor
I can delete the Actor

Scenario: Delete Actor
	Given I have a def "<def>"
	And I have a actor id"<id>"
	And The actor exists "<exists>"
	When I delete the actor
	Then The response is "<response>"
	And If the response has validation issues I see the "<responseErrors>" in the response
 
Examples:
	| def                   | response   | responseErrors | id                                   | exists |
	| success               | Success    |                | 038d8e7f-f18f-4a8e-8b3c-3b6a6889fed9 | true   |
	| not found             | NotFound   |                | 038d8e7f-f18f-4a8e-8b3c-3b6a6889fed9 | false  |