@patchChatSessionCommand
Feature: Patch ChatSession Command
As a creator
I can patch a chatSession 

Scenario: Patch Chat Session
	Given I have a def "<def>"
	And I have a chat session id "<id>"
	And the chat session exists "<chatSessionExists>"
	And I have a new chat session title "<title>"
	When I patch the chatSession
	Then The response is "<response>"
	And If the response has validation issues I see the "<responseErrors>" in the response
 
Examples:
	| def                     | response   | responseErrors | id                                   | chatSessionExists | title         |
	| success : patch title   | Success    |                | 038d8e7f-f18f-4a8e-8b3c-3b6a6889fed9 | true              | New Title     |
	| bad request: empty id   | BadRequest | Id             | 00000000-0000-0000-0000-000000000000 | false             | Changed Title |
	| not found : patch title | NotFound   |                | 038d8e7f-f18f-4a8e-8b3c-3b6a6889fed9 | false             | Title         |