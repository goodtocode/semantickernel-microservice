@getTextImageQuery
Feature: Get Text Image Query
As a actor
When I select an existing text image
I can see the text image responses

Scenario: Get text image
	Given I have a definition "<def>"
	And I have a text image id "<id>"
	And I the text image exists "<textPromptExists>"
	When I get a text image
	Then The response is "<response>"
	And If the response has validation issues I see the "<responseErrors>" in the response
	And If the response is successful the response has a Key

Examples:
	| def                   | response   | responseErrors | id                                   | textPromptExists |
	| success               | Success    |                | 038d8e7f-f18f-4a8e-8b3c-3b6a6889fed9 | true              |
	| not found             | NotFound   |                | 048d8e7f-f18f-4a8e-8b3c-3b6a6889fed9 | false             |
	| bad request: empty id | BadRequest | Id             | 00000000-0000-0000-0000-000000000000 | false             |