@createChatMessageCommand
Feature: Create Chat Message Command
As a chat user
When I start a new Chat Message and enter an initial message
Then I should see the Chat Message created with the initial response

Scenario: Create Chat Message
	Given I have a def "<def>"
	And I have a initial message "<message>"
	And I have a Chat Message id "<id>"
	And The Chat Message exists "<ChatMessageExists>"
	When I create a Chat Message with the message 
	Then I see the Chat Message created with the initial response "<response>"
	And if the response has validation issues I see the "<responseErrors>" in the response
 
Examples:
	| def                        | response   | responseErrors | id                                   | ChatMessageExists | message                                                                                                                     |
	| success                    | Success    |                | 00000000-0000-0000-0000-000000000000 | true              | Hello, I am interested in an interactive Chat Message.                                                                      |
	| success actor plugin      | Success    |                | 00000000-0000-0000-0000-000000000000 | true              | Please call get_author that Returns the actor's name for the specified actor ID, or 'Actor not found' if no match exists |
	| success session plugin     | Success    |                | 00000000-0000-0000-0000-000000000000 | true              | Please call list_sessions that Lists all sessions, optionally by date                                                       |
	| success messages plugin    | Success    |                | 00000000-0000-0000-0000-000000000000 | true              | Please call list_messages that Lists all sessions, optionally by date                                                       |
	| bad request: empty message | BadRequest | Message        | 00000000-0000-0000-0000-000000000000 | false             |                                                                                                                             |
	| already exists             | Error      |                | 038d8e7f-f18f-4a8e-8b3c-3b6a6889fed9 | true              | Hello, I am interested in an interactive Chat Message.                                                                      |