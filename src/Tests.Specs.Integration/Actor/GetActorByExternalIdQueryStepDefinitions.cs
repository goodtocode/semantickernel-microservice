using Goodtocode.SemanticKernel.Core.Application.Actor;
using Goodtocode.SemanticKernel.Core.Domain.Actor;

namespace Goodtocode.SemanticKernel.Specs.Integration.Actor;

[Binding]
[Scope(Tag = "getAuthorByOwnerIdQuery")]
public class GetActorByOwnerIdQueryStepDefinitions : TestBase
{
    private bool _exists;
    private ActorDto? _response;

    [Given(@"I have a definition ""([^""]*)""")]
    public void GivenIHaveADefinition(string def)
    {
        base.def = def;
    }

    [Given("I have a Actor External Id")]
    public void GivenIHaveAAuthorExternalId()
    {
        userInfo.OwnerId.ShouldNotBeEmpty();
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
            var actor = ActorEntity.Create(userInfo.OwnerId, userInfo.OwnerId, Guid.NewGuid(), "John", "Doe", "jdoe@goodtocode.com");
            context.Actors.Add(actor);
            await context.SaveChangesAsync(CancellationToken.None);
        }

        var request = new GetMyActorQuery() 
        {
            UserInfo = userInfo
        };

        var validator = new GetMyActorQueryValidator();
        validationResponse = validator.Validate(request);
        if (validationResponse.IsValid)
            try
            {
                var handler = new GetAuthorByOwnerIdQueryHandler(context);
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
