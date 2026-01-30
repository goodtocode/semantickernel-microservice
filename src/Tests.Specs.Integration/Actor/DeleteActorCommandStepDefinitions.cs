using Goodtocode.SemanticKernel.Core.Application.Actor;
using Goodtocode.SemanticKernel.Core.Domain.Actor;

namespace Goodtocode.SemanticKernel.Specs.Integration.Actor
{
    [Binding]
    [Scope(Tag = "deleteAuthorCommand")]
    public class DeleteActorCommandStepDefinitions : TestBase
    {
        private bool _exists;
        private Guid _id;

        [Given(@"I have a def ""([^""]*)""")]
        public void GivenIHaveADef(string def)
        {
            base.def = def;
        }

        [Given(@"I have a actor id""([^""]*)""")]
        public void GivenIHaveAAuthorId(string id)
        {
            Guid.TryParse(id, out _id).ShouldBeTrue();
        }

        [Given(@"The actor exists ""([^""]*)""")]
        public void GivenTheAuthorExists(string exists)
        {
            bool.TryParse(exists, out _exists).ShouldBeTrue();
        }

        [When(@"I delete the actor")]
        public async Task WhenIDeleteTheAuthor()
        {
            if (_exists)
            {
                var actor = ActorEntity.Create(_id, _id, Guid.NewGuid(), "John", "Doe", "jdoe@goodtocode.com");
                context.Actors.Add(actor);
                await context.SaveChangesAsync(CancellationToken.None);
            }

            var request = new DeleteActorByOwnerIdCommand()
            {
                OwnerId = _id
            };

            var validator = new DeleteActorByOwnerIdCommandValidator();
            validationResponse = await validator.ValidateAsync(request);

            if (validationResponse.IsValid)
                try
                {
                    var handler = new DeleteAuthorByOwnerIdCommandHandler(context);
                    await handler.Handle(request, CancellationToken.None);
                    responseType = CommandResponseType.Successful;
                }
                catch (Exception e)
                {
                    HandleAssignResponseType(e);
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
    }
}
