using Goodtocode.SemanticKernel.Core.Application.Image;
using Goodtocode.SemanticKernel.Core.Domain.Image;
using System.Text;

namespace Goodtocode.SemanticKernel.Specs.Integration.Image
{
    [Binding]
    [Scope(Tag = "deleteTextImageCommand")]
    public class DeleteTextImageCommandStepDefinitions : TestBase
    {
        private Guid _id;
        private bool _exists;

        [Given(@"I have a def ""([^""]*)""")]
        public void GivenIHaveADef(string def)
        {
            _def = def;
        }

        [Given(@"I have a text image id""([^""]*)""")]
        public void GivenIHaveATextImageKey(string id)
        {
            Guid.TryParse(id, out _id).Should().BeTrue();
        }

        [Given(@"The text image exists ""([^""]*)""")]
        public void GivenThetextImageExists(string exists)
        {
            bool.TryParse(exists, out _exists).Should().BeTrue();
        }

        [When(@"I delete the text image")]
        public async Task WhenIDeleteTheTextImage()
        {
            var request = new DeleteTextImageCommand()
            {
                Id = _id
            };

            if (_exists)
            {
                var textImage = new TextImageEntity()
                {
                    Id = _id,
                    Description = "Image of a simple geometric design consisting of two yellow squares and one blue square. " +
                        "The blue square is placed at a 45-degree angle, positioned centrally below the two yellow squares, creating a symmetrical arrangement. " +
                        "Each square is connected by what appears to be black lines or sticks, suggesting they may represent nodes or elements in a network or structure. " +
                        "The background is white, which contrasts with the bright colors of the squares.",
                    Width = 1024,
                    Height = 1024,
                    ImageBytes = [0x01, 0x02, 0x03, 0x04],
                    Timestamp = DateTime.UtcNow
                };
                _context.TextImages.Add(textImage);
                await _context.SaveChangesAsync(CancellationToken.None);
            }

            var validator = new DeleteTextImageCommandValidator();
            _validationResponse = await validator.ValidateAsync(request);

            if (_validationResponse.IsValid)
                try
                {
                    var handler = new DeleteTextImageCommandHandler(_context);
                    await handler.Handle(request, CancellationToken.None);
                    _responseType = CommandResponseType.Successful;
                }
                catch (Exception e)
                {
                    HandleAssignResponseType(e);
                }
            else
                _responseType = CommandResponseType.BadRequest;
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
