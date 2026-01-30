@getAuthorChatSessionQuery
Feature: Get Actor Chat Session Query
As a chat user actor
When I select an existing chat session
I can get the chat history messages

Scenario: Get actor chat session
	Given I have a definition "<def>"
	And I have a actor id "<id>"
	And I have a chat session id "<chatSessionId>"
	And I the chat session exists "<chatSessionExists>"
	When I get a chat session
	Then The response is "<response>"
	And If the response has validation issues I see the "<responseErrors>" in the response
	And If the response is successful the response has a Id

Examples:
	| def                   | response   | responseErrors | id                                   | chatSessionId                        | chatSessionExists |
	| success               | Success    |                | 098d8e7f-f18f-4a8e-8b3c-3b6a6889fed9 | 045d8e7f-f18f-4a8e-8b3c-3b6a6889fed9 | true              |
	| not found             | NotFound   |                | 023d8e7f-f18f-4a8e-8b3c-3b6a6889fed9 | 078d8e7f-f18f-4a8e-8b3c-3b6a6889fed9 | false             |
