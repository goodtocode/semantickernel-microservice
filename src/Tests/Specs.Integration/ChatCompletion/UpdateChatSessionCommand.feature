@updateChatSessionCommand
Feature: Update Chat Session Command
As a chat session owner
When I edit a chat session
I am able to change or add to the chat session

Scenario: Update Chat Session
	Given I have a def "<def>"
	And I have a chat session id "<id>"
	And the chat session exists "<exists>"
	When I update the chat session
	Then The response is "<response>"
	And If the response has validation issues I see the "<responseErrors>" in the response
 
Examples:
	| def                             | response   | responseErrors | id                                  | exists |
	| success                         | Success    |                | 038d8e7f-f18f-4a8e-8b3c-3b6a6889fed9 | true   |
	| not found                       | NotFound   |                | 038d8e7f-f18f-4a8e-8b3c-3b6a6889fed9 | false  |
	| bad request: empty id          | BadRequest | Id            | 00000000-0000-0000-0000-000000000000 | false  |