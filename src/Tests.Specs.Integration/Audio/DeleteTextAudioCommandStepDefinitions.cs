using System.Text;
using Goodtocode.SemanticKernel.Core.Application.Audio;
using Goodtocode.SemanticKernel.Core.Domain.Audio;

namespace Goodtocode.SemanticKernel.Specs.Integration.Audio
{
    [Binding]
    [Scope(Tag = "deleteTextAudioCommand")]
    public class DeleteTextAudioCommandStepDefinitions : TestBase
    {
        private Guid _id;
        private bool _exists;

        [Given(@"I have a def ""([^""]*)""")]
        public void GivenIHaveADef(string def)
        {
            base.def = def;
        }

        [Given(@"I have a text audio id""([^""]*)""")]
        public void GivenIHaveATextAudioKey(string id)
        {
            Guid.TryParse(id, out _id).Should().BeTrue();
        }

        [Given(@"The text audio exists ""([^""]*)""")]
        public void GivenThetextAudioExists(string exists)
        {
            bool.TryParse(exists, out _exists).Should().BeTrue();
        }

        [When(@"I delete the text audio")]
        public async Task WhenIDeleteTheTextAudio()
        {
            var request = new DeleteTextAudioCommand()
            {
                Id = _id
            };

            if (_exists)
            {
                var textAudio = TextAudioEntity.Create(_id, Guid.NewGuid(), "Audio of a simple geometric design consisting of two yellow squares and one blue square. " +
                        "The blue square is placed at a 45-degree angle, positioned centrally below the two yellow squares, creating a symmetrical arrangement. " +
                        "Each square is connected by what appears to be black lines or sticks, suggesting they may represent nodes or elements in a network or structure. " +
                        "The background is white, which contrasts with the bright colors of the squares.",
                    new ReadOnlyMemory<byte>([0x01, 0x02, 0x03, 0x04]));
                context.TextAudio.Add(textAudio);
                await context.SaveChangesAsync(CancellationToken.None);
            }

            var validator = new DeleteTextAudioCommandValidator();
            validationResponse = await validator.ValidateAsync(request);

            if (validationResponse.IsValid)
                try
                {
                    var handler = new DeleteTextAudioCommandHandler(context);
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
