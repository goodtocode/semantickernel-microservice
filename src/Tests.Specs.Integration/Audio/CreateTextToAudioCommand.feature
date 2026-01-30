@createTextToAudioCommand
Feature: Create Text To Audio Command
As a actor
When I start a new text audio and enter an initial prompt
Then I should see the text audio created with the initial response

Scenario: Create Text Audio
	Given I have a def "<def>"
	And I have a initial prompt "<prompt>"
	And I have a text audio id "<id>"
	And The text audio exists "<textAudioExists>"
	When I create a text audio with the prompt 
	Then I see the text audio created with the initial response "<response>"
	And if the response has validation issues I see the "<responseErrors>" in the response
 
Examples:
	| def                       | response   | responseErrors | id                                   | textAudioExists | prompt                                   |
	| success                   | Success    |                | 00000000-0000-0000-0000-000000000000 | false           | Hello, I am a voice generated from text. |
	| bad request: empty propmt | BadRequest | Prompt         | 00000000-0000-0000-0000-000000000000 | false           |                                          |
	| already exists            | Error      |                | 038d8e7f-f18f-4a8e-8b3c-3b6a6889fed9 | true            | Hello, I am a voice generated from text. |