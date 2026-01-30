using Goodtocode.SemanticKernel.Core.Application.Actor;
using Goodtocode.SemanticKernel.Core.Domain.Actor;

namespace Goodtocode.SemanticKernel.Specs.Integration.Actor
{
    [Binding]
    [Scope(Tag = "updateAuthorCommand")]
    public class UpdateActorCommandStepDefinitions : TestBase
    {
        private bool _exists;
        private Guid _id;

        [Given(@"I have a def ""([^""]*)""")]
        public void GivenIHaveADef(string def)
        {
            base.def = def;
        }

        [Given(@"I have a Actor id ""([^""]*)""")]
        public void GivenIHaveAAuthorId(string id)
        {
            Guid.TryParse(id, out _id).ShouldBeTrue();
        }

        [Given(@"the Actor exists ""([^""]*)""")]
        public void GivenTheAuthorExists(string exists)
        {
            bool.TryParse(exists, out _exists).ShouldBeTrue();
        }

        [When(@"I update the Actor")]
        public async Task WhenIUpdateTheAuthor()
        {
            if (_exists)
            {
                var actor = ActorEntity.Create(_id, _id, Guid.NewGuid(), "John", "Doe", "jdoe@goodtocode.com");
                context.Actors.Add(actor);
                await context.SaveChangesAsync(CancellationToken.None);
            }

            var request = new UpdateActorCommand()
            {
                OwnerId = _id,
                Name = "Joe Doe"
            };

            var validator = new UpdateActorCommandValidator();
            validationResponse = await validator.ValidateAsync(request);

            if (validationResponse.IsValid)
                try
                {
                    var handler = new UpdateAuthorCommandHandler(context);
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
