@createChatSessionCommand
Feature: Create Chat Session Command
As a chat user
When I start a new chat session and enter an initial message
Then I should see the chat session created with the initial response

Scenario: Create Chat Session
	Given I have a def "<def>"
	And I have a initial message "<message>"
	And I have a chat session id "<id>"
	And The chat session exists "<chatSessionExists>"
	When I create a chat session with the message 
	Then I see the chat session created with the initial response "<response>"
	And if the response has validation issues I see the "<responseErrors>" in the response
 
Examples:
	| def                        | response   | responseErrors | id                                  | chatSessionExists | message                                                |
	| success                    | Success    |                | 00000000-0000-0000-0000-000000000000 | false             | Hello, I am interested in an interactive chat session. |
	| bad request: empty message | BadRequest | Message        | 00000000-0000-0000-0000-000000000000 | false             |                                                        |
	| already exists             | Error      |                | 038d8e7f-f18f-4a8e-8b3c-3b6a6889fed9 | true              | Hello, I am interested in an interactive chat session. |