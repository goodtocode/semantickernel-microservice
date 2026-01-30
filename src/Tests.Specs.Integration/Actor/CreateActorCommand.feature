@createAuthorCommand
Feature: Create Actor Command
As a owner
When I want to save a new Actor
Then I should see the Actor created with the initial response

Scenario: Create Actor
	Given I have a def "<def>"
	And I have a name "<name>"
	And I have a Actor id "<id>"
	And I have a External id "<ownerId>"
	And I have a Email "<email>"
	And The Actor exists "<exists>"
	When I create a actor
	Then I see the Actor created with the initial response "<response>"
	And if the response has validation issues I see the "<responseErrors>" in the response
 
Examples:
	| def                           | response   | responseErrors | id                                   | ownerId                           | exists | name     | email               |
	| success                       | Success    |                | 00000000-0000-0000-0000-000000000000 | 938d8e7f-f18f-4a8e-8b3c-3b6a6889fed9 | false  | John Doe | jdoe@goodtocode.com |
	| already exists                | Error      |                | 038d8e7f-f18f-4a8e-8b3c-3b6a6889fed9 | 938d8e7f-f18f-4a8e-8b3c-3b6a6889fed9 | true   | Jane Doe | jane@goodtocode.com |