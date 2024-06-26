﻿@getAuthorQuery
Feature: Get Author Query
As a author owner
When I select an existing Author
I can see the author detail

Scenario: Get Author
	Given I have a definition "<def>"
	And I have a Author id "<id>"
	And I the Author exists "<exists>"
	When I get a Author
	Then The response is "<response>"
	And If the response has validation issues I see the "<responseErrors>" in the response
	And If the response is successful the response has a Id	

Examples:
	| def                   | response   | responseErrors | id                                   | exists |
	| success               | Success    |                | 038d8e7f-f18f-4a8e-8b3c-3b6a6889fed9 | true   |
	| not found             | NotFound   |                | 048d8e7f-f18f-4a8e-8b3c-3b6a6889fed9 | false  |
	| bad request: empty id | BadRequest | AuthorId       | 00000000-0000-0000-0000-000000000000 | false  |