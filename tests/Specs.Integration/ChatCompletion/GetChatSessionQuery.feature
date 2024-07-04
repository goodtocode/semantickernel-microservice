@getChatSessionQuery
Feature: Get Chat Session Query
As a chat user
When I select an existing chat session
I can see the chat history messages

Scenario: Get chat session
	Given I have a definition "<def>"
	And I have a chat session id "<id>"
	And I the chat session exists "<chatSessionExists>"
	And I have a expected chat session count "<expectedChatSessionCount>"
	When I get a chat session
	Then The response is "<response>"
	And If the response has validation issues I see the "<responseErrors>" in the response
	And If the response is successful the response has a Id
	And If the response is successful the response has a count matching "<expectedChatSessionCount>"	

Examples:
	| def                   | response   | responseErrors | id                                   | chatSessionExists | expectedChatSessionCount |
	| success               | Success    |                | 038d8e7f-f18f-4a8e-8b3c-3b6a6889fed9 | true              | 2                        |
	| not found             | NotFound   |                | 048d8e7f-f18f-4a8e-8b3c-3b6a6889fed9 | false             | 0                        |
	| bad request: empty id | BadRequest | Id             | 00000000-0000-0000-0000-000000000000 | false             | 0                        |