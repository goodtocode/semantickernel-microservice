@getTextPromptQuery
Feature: Get Text Prompt Query
As a author
When I select an existing text prompt
I can see the text prompt responses

Scenario: Get text prompt
	Given I have a definition "<def>"
	And I have a text prompt id "<id>"
	And I the text prompt exists "<textPromptExists>"
	And I have a expected text prompt count "<expectedTextPromptCount>"
	When I get a text prompt
	Then The response is "<response>"
	And If the response has validation issues I see the "<responseErrors>" in the response
	And If the response is successful the response has a Key
	And If the response is successful the response has a count matching "<expectedTextPromptCount>"	

Examples:
	| def                   | response   | responseErrors | id                                   | textPromptExists | expectedTextPromptCount |
	| success               | Success    |                | 038d8e7f-f18f-4a8e-8b3c-3b6a6889fed9 | true              | 2                        |
	| not found             | NotFound   |                | 048d8e7f-f18f-4a8e-8b3c-3b6a6889fed9 | false             | 0                        |
	| bad request: empty id | BadRequest | Id             | 00000000-0000-0000-0000-000000000000 | false             | 0                        |