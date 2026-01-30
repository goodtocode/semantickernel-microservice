@getAuthorByOwnerIdQuery
Feature: Get Actor By External Id Query
As a actor owner
When I select an existing Actor
I can see the actor detail

Scenario: Get Actor By External Id
	Given I have a definition "<def>"
	And I have a Actor External Id
	And the Actor exists "<exists>"
	When I get the Actor
	Then The response is "<response>"
	And If the response has validation issues I see the "<responseErrors>" in the response
	And If the response is successful the response has a Id	

Examples:
	| def       | response | responseErrors | exists |
	| success   | Success  |                | true   |
	| not found | NotFound |                | false  |