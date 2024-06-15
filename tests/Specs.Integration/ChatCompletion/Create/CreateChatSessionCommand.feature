@createChatSessionCommand
Feature: Create Chat Session Command
As a chat user
When I start a new chat session and enter an initial message
Then I should see the chat session created with the initial response

Scenario: Create Chat Session
	Given I have a def "<def>"
	And I have a initial message "<message>"
	And I have a chat session key "<key>"
	And The chat session exists "<chatSessionExists>"
	When I create a chat sesion with the message 
	Then I see the chat session created with the initial response "<response>"
	And if the response has validation issues I see the "<responseErrors>" in the response
	And chat completion returns with content "<content>"
	And chat completion returns with the according role "<role>"
 
Examples:
	| def                        | response   | responseErrors | key                                  | chatSessionExists | message                                                | content                        | responseRole |
	| success                    | Success    |                | 00000000-0000-0000-0000-000000000000 | false             | Hello, I am interested in an interactive chat session. | Certainly! I’m here to assist. | Assistant    |
	| bad request: empty message | BadRequest | Message        | 00000000-0000-0000-0000-000000000000 | false             |                                                        |                                |              |
	| already exists             | BadRequest | Key            | 038d8e7f-f18f-4a8e-8b3c-3b6a6889fed9 | true              | Hello, I am interested in an interactive chat session. | Certainly! I’m here to assist. | Assistant    |