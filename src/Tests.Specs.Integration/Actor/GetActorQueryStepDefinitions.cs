using Goodtocode.SemanticKernel.Core.Application.Actor;
using Goodtocode.SemanticKernel.Core.Domain.Actor;

namespace Goodtocode.SemanticKernel.Specs.Integration.Actor;

[Binding]
[Scope(Tag = "getAuthorQuery")]
public class GetActorQueryStepDefinitions : TestBase
{
    private Guid _id;
    private bool _exists;
    private ActorDto? _response;

    [Given(@"I have a definition ""([^""]*)""")]
    public void GivenIHaveADefinition(string def)
    {
        base.def = def;
    }

    [Given(@"I have a Actor id ""([^""]*)""")]
    public void GivenIHaveAAuthorId(string authorId)
    {
        if (string.IsNullOrWhiteSpace(authorId)) return;
        Guid.TryParse(authorId, out _id).ShouldBeTrue();
    }

    [Given(@"the Actor exists ""([^""]*)""")]
    public void GivenITheAuthorExists(string exists)
    {
        bool.TryParse(exists, out _exists).ShouldBeTrue();
    }

    [When(@"I get the Actor")]
    public async Task WhenIGetAAuthor()
    {
        if (_exists)
        {
            var actor = ActorEntity.Create(_id, Guid.NewGuid(), Guid.NewGuid(), "John", "Doe", "jdoe@goodtocode.com");
            context.Actors.Add(actor);
            await context.SaveChangesAsync(CancellationToken.None);
        }

        var request = new GetActorQuery()
        {
            ActorId = _id
        };

        var validator = new GetActorQueryValidator();
        validationResponse = validator.Validate(request);
        if (validationResponse.IsValid)
            try
            {
                var handler = new GetAuthorQueryHandler(context);
                _response = await handler.Handle(request, CancellationToken.None);
                responseType = CommandResponseType.Successful;
            }
            catch (Exception e)
            {
                responseType = HandleAssignResponseType(e);
            }
        else
            responseType = CommandResponseType.BadRequest;
    }

    [Then(@"The response is ""([^""]*)""")]
    public void ThenTheResponseIs(string response)
    {
        HandleHasResponseType(response);
    }

    [Then(@"If the response has validation issues I see the ""([^""]*)"" in the response")]
    public void ThenIfTheResponseHasValidationIssuesISeeTheInTheResponse(string expectedErrors)
    {
        HandleExpectedValidationErrorsAssertions(expectedErrors);
    }

    [Then(@"If the response is successful the response has a Id")]
    public void ThenIfTheResponseIsSuccessfulTheResponseHasAKey()
    {
        if (responseType != CommandResponseType.Successful) return;
        _response?.Id.ShouldNotBeEmpty();
    }
}
