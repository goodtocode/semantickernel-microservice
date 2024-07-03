@deleteTextPromptCommand
Feature: Delete Text Prompt Command
As a text prompt owner
When I select a text prompt
I can delete the text prompt

Scenario: Delete Text Prompt
	Given I have a def "<def>"
	And I have a text prompt id"<id>"
	And The text prompt exists "<exists>"
	When I delete the text prompt
	Then The response is "<response>"
	And If the response has validation issues I see the "<responseErrors>" in the response
 
Examples:
	| def                   | response   | responseErrors | id                                   | exists |
	| success               | Success    |                | 038d8e7f-f18f-4a8e-8b3c-3b6a6889fed9 | true   |
	| not found             | NotFound   |                | 038d8e7f-f18f-4a8e-8b3c-3b6a6889fed9 | false  |
	| bad request: empty id | BadRequest | Id             | 00000000-0000-0000-0000-000000000000 | false  |