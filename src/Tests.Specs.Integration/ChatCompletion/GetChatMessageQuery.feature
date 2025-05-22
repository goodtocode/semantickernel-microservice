@getChatMessageQuery
Feature: Get Chat Message Query
As a chat user
When I select an existing Chat Message
I can see the chat history messages

Scenario: Get Chat Message
	Given I have a definition "<def>"
	And I have a Chat Message id "<id>"
	And I the Chat Message exists "<ChatMessageExists>"
	And I have a expected Chat Message contents "<expectedChatMessageContents>"
	When I get a Chat Message
	Then The response is "<response>"
	And If the response has validation issues I see the "<responseErrors>" in the response
	And If the response is successful the response has a Id
	And If the response is successful the response has contents matching "<expectedChatMessageContents>"	

Examples:
	| def                   | response   | responseErrors | id                                   | ChatMessageExists | expectedChatMessageContents |
	| success               | Success    |                | 038d8e7f-f18f-4a8e-8b3c-3b6a6889fed9 | true              | Hello, I am interested in an interactive chat session.                        |
	| not found             | NotFound   |                | 048d8e7f-f18f-4a8e-8b3c-3b6a6889fed9 | false             |                         |
	| bad request: empty id | BadRequest | Id             | 00000000-0000-0000-0000-000000000000 | false             |                       |