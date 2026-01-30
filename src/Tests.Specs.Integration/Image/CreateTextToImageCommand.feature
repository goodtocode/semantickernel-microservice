@createTextToImageCommand
Feature: Create Text To Image Command
As a actor
When I start a new text image and enter an initial prompt
Then I should see the text image created with the initial response

Scenario: Create Text Image
	Given I have a def "<def>"
	And I have a initial prompt "<prompt>"
	And I have a text image id "<id>"
	And The text image exists "<textImageExists>"
	When I create a text image with the prompt 
	Then I see the text image created with the initial response "<response>"
	And if the response has validation issues I see the "<responseErrors>" in the response
 
Examples:
	| def                       | response   | responseErrors | id                                   | textImageExists | prompt                                              |
	| success                   | Success    |                | 00000000-0000-0000-0000-000000000000 | false           | Create an image of a triangle, square and a circle. |
	| bad request: empty propmt | BadRequest | Prompt         | 00000000-0000-0000-0000-000000000000 | false           |                                                     |
	| already exists            | Error      |                | 038d8e7f-f18f-4a8e-8b3c-3b6a6889fed9 | true            | Create an image of a triangle, square and a circle. |