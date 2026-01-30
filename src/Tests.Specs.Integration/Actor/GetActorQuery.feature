@getAuthorQuery
Feature: Get Actor Query
As a actor owner
When I select an existing Actor
I can see the actor detail

Scenario: Get Actor
	Given I have a definition "<def>"
	And I have a Actor id "<id>"
	And the Actor exists "<exists>"
	When I get the Actor
	Then The response is "<response>"
	And If the response has validation issues I see the "<responseErrors>" in the response
	And If the response is successful the response has a Id	

Examples:
	| def                   | response   | responseErrors | id                                   | exists |
	| success               | Success    |                | 038d8e7f-f18f-4a8e-8b3c-3b6a6889fed9 | true   |
	| not found             | NotFound   |                | 048d8e7f-f18f-4a8e-8b3c-3b6a6889fed9 | false  |
	| bad request: empty id | BadRequest | ActorId       | 00000000-0000-0000-0000-000000000000 | false  |