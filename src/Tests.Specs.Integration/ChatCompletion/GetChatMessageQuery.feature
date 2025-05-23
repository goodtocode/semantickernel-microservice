@getChatMessageQuery
Feature: Get Chat Message Query
As a chat user
When I select an existing Chat Message
I can see the chat history messages

Scenario: Get Chat Message
	Given I have a definition "<def>"
	And I have a Chat Message id "<id>"
	And The Chat Message exists "<chatMessageExists>"
	When I get a Chat Message
	Then The response is "<response>"
	And If the response has validation issues I see the "<responseErrors>" in the response
	And If the response is successful the response has a Id

Examples:
	| def                   | response   | responseErrors | id                                   | chatMessageExists |
	| success               | Success    |                | 038d8e7f-f18f-4a8e-8b3c-3b6a6889fed9 | true              |
	| not found             | NotFound   |                | 048d8e7f-f18f-4a8e-8b3c-3b6a6889fed9 | false             |
	| bad request: empty id | BadRequest | Id             | 00000000-0000-0000-0000-000000000000 | false             |