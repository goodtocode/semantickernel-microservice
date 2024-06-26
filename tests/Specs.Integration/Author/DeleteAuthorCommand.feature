﻿@deleteAuthorCommand
Feature: Delete Author Command
As a Author owner
When I select a Author
I can delete the Author

Scenario: Delete Author
	Given I have a def "<def>"
	And I have a author id"<id>"
	And The author exists "<exists>"
	When I delete the author
	Then The response is "<response>"
	And If the response has validation issues I see the "<responseErrors>" in the response
 
Examples:
	| def                   | response   | responseErrors | id                                   | exists |
	| success               | Success    |                | 038d8e7f-f18f-4a8e-8b3c-3b6a6889fed9 | true   |
	| not found             | NotFound   |                | 038d8e7f-f18f-4a8e-8b3c-3b6a6889fed9 | false  |
	| bad request: empty id | BadRequest | Id             | 00000000-0000-0000-0000-000000000000 | false  |