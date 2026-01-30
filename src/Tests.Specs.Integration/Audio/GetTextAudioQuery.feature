@getTextAudioQuery
Feature: Get Text Audio Query
As a actor
When I select an existing text audio
I can see the text audio responses

Scenario: Get text audio
	Given I have a definition "<def>"
	And I have a text audio id "<id>"
	And I the text audio exists "<textPromptExists>"
	When I get a text audio
	Then The response is "<response>"
	And If the response has validation issues I see the "<responseErrors>" in the response
	And If the response is successful the response has a Key

Examples:
	| def                   | response   | responseErrors | id                                   | textPromptExists |
	| success               | Success    |                | 038d8e7f-f18f-4a8e-8b3c-3b6a6889fed9 | true              |
	| not found             | NotFound   |                | 048d8e7f-f18f-4a8e-8b3c-3b6a6889fed9 | false             |
	| bad request: empty id | BadRequest | Id             | 00000000-0000-0000-0000-000000000000 | false             |