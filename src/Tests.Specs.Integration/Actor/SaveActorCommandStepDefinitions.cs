using Goodtocode.SemanticKernel.Core.Application.Actor;
using Goodtocode.SemanticKernel.Core.Domain.Actor;

namespace Goodtocode.SemanticKernel.Specs.Integration.Actor;

[Binding]
[Scope(Tag = "saveAuthorCommand")]
public class SaveActorCommandStepDefinitions : TestBase
{
    private string _name = string.Empty;
    private Guid _id;
    private bool _exists;
    private Guid _ownerId = Guid.NewGuid();
    private readonly Guid _tenantId = Guid.NewGuid();
    private string _email = string.Empty;

    [Given(@"I have a def ""([^""]*)""")]
    public void GivenIHaveADef(string def)
    {
        base.def = def;
    }

    [Given(@"I have a name ""([^""]*)""")]
    public void GivenIHaveAName(string name)
    {
        _name = name;
    }

    [Given("I have a Email {string}")]
    public void GivenIHaveAEmail(string email)
    {
        _email = email;
    }

    [Given(@"I have a Actor id ""([^""]*)""")]
    public void GivenIHaveAAuthorId(string id)
    {
        _id = Guid.Parse(id);
    }

    [Given("I have a External id {string}")]
    public void GivenIHaveAExternalId(string ownerId)
    {
        _ownerId = Guid.Parse(ownerId);
    }

    [Given(@"The Actor exists ""([^""]*)""")]
    public void GivenTheAuthorExists(string exists)
    {
        bool.TryParse(exists, out _exists).ShouldBeTrue();
    }

    [When(@"I create a actor")]
    public async Task WhenICreateAAuthor()
    {
        if (_exists)
        {
            var actor = ActorEntity.Create(_id, _ownerId, _tenantId, "John", "Doe", "jdoe@goodtocode.com");
            context.Actors.Add(actor);
            await context.SaveChangesAsync(CancellationToken.None);
        }

        var request = new SaveMyActorCommand()
        {
            TenantId = _tenantId,
            FirstName = _name.Split(" ").FirstOrDefault(),
            LastName = _name.Split(" ").LastOrDefault(),
            Email = _email,
            UserInfo = userInfo
        };

        var validator = new SaveMyActorCommandValidator();
        validationResponse = await validator.ValidateAsync(request);

        if (validationResponse.IsValid)
        {
            try
            {
                var handler = new SaveAuthorCommandHandler(context);
                await handler.Handle(request, CancellationToken.None);
                responseType = CommandResponseType.Successful;
            }
            catch (Exception e)
            {
                HandleAssignResponseType(e);
            }
        }
        else
            responseType = CommandResponseType.BadRequest;
    }

    [Then(@"I see the Actor created with the initial response ""([^""]*)""")]
    public void ThenISeeTheAuthorCreatedWithTheInitialResponse(string response)
    {
        HandleHasResponseType(response);
    }

    [Then(@"if the response has validation issues I see the ""([^""]*)"" in the response")]
    public void ThenIfTheResponseHasValidationIssuesISeeTheInTheResponse(string expectedErrors)
    {
        HandleExpectedValidationErrorsAssertions(expectedErrors);
    }

}