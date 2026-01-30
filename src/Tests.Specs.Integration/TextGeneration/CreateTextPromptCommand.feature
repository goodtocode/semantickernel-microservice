@createTextPromptCommand
Feature: Create Text Prompt Command
As a actor
When I start a new text prompt and enter an initial prompt
Then I should see the text prompt created with the initial response

Scenario: Create Text Prompt
	Given I have a def "<def>"
	And I have a initial prompt "<prompt>"
	And I have a text prompt id "<id>"
	And The text prompt exists "<TextPromptExists>"
	When I create a text prompt with the prompt 
	Then I see the text prompt created with the initial response "<response>"
	And if the response has validation issues I see the "<responseErrors>" in the response
 
Examples:
	| def                       | response   | responseErrors | id                                   | TextPromptExists | prompt                   |
	| success                   | Success    |                | 00000000-0000-0000-0000-000000000000 | false            | Tell me a bedtime story. |
	| bad request: empty propmt | BadRequest | Prompt         | 00000000-0000-0000-0000-000000000000 | false            |                          |
	| already exists            | Error      |                | 038d8e7f-f18f-4a8e-8b3c-3b6a6889fed9 | true             | Tell me a bedtime story. |