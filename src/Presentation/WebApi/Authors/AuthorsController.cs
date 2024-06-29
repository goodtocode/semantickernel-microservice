using Goodtocode.SemanticKernel.Core.Application.Author;
using Goodtocode.SemanticKernel.Core.Application.Common.Models;
using Goodtocode.SemanticKernel.Presentation.WebApi.Common;

namespace Goodtocode.SemanticKernel.Presentation.WebApi.Author;

/// <summary>
/// Author endpoints to create a chat, continue a chat, delete a chat and retrieve chat history
/// </summary>
[ApiController]
[ApiConventionType(typeof(DefaultApiConventions))]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
public class AuthorController : ApiControllerBase
{
    /// <summary>Get Author session with history</summary>
    /// <remarks>
    /// Sample request:
    ///
    ///        "Key": 60fb5e99-3a78-43df-a512-7d8ff498499e
    ///        "api-version":  1.0
    /// 
    /// </remarks>
    /// <returns>
    /// AuthorDto
    ///     { 
    ///         Key: 1efb5e99-3a78-43df-a512-7d8ff498499e
    ///         Name: John Doe
    ///     }
    /// </returns>
    [HttpGet("{key}", Name = "GetAuthorQuery")]
    [ProducesResponseType(typeof(AuthorDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<AuthorDto> Get(Guid key)
    {
        return await Mediator.Send(new GetAuthorQuery
        {
            Id = key
        });
    }

    /// <summary>
    /// Creates new Author session with empty history
    /// </summary>
    /// <remarks>
    /// Sample request:
    ///
    ///     HttpPost Body
    ///     {
    ///        "Key": 00000000-0000-0000-0000-000000000000,
    ///        "Name":  "John Doe"
    ///     }
    ///
    ///     "version":  1.0
    /// </remarks>
    /// <param name="command"></param>
    /// <returns></returns>
    [HttpPost(Name = "CreateAuthorCommand")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> Post(CreateAuthorCommand command)
    {
        var response = await Mediator.Send(command);
        return CreatedAtAction(nameof(Get), new { response.Id }, command);
    }

    /// <summary>
    /// Update Author Command
    /// </summary>
    /// <remarks>
    /// Sample request:
    ///
    ///     HttpPut Body
    ///     {
    ///        "Key": "60fb5e99-3a78-43df-a512-7d8ff498499e",
    ///        "Name":  "John Doe",
    ///     }
    ///
    ///     "version":  1.0
    /// </remarks>
    /// <param name="command"></param>
    /// <returns></returns>
    [HttpPut(Name = "UpdateAuthorCommand")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> Put(UpdateAuthorCommand command)
    {
        await Mediator.Send(command);

        return NoContent();
    }

    /// <summary>Remove Author Command</summary>
    /// <remarks>
    /// Sample request:
    ///
    ///        "Key": 60fb5e99-3a78-43df-a512-7d8ff498499e
    ///        "api-version":  1.0
    /// 
    /// </remarks>
    /// <returns>NoContent</returns>
    [HttpDelete("{key}", Name = "RemoveAuthorCommand")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesDefaultResponseType]
    public async Task<IActionResult> Delete(Guid key)
    {
        await Mediator.Send(new DeleteAuthorCommand() { Id = key });

        return NoContent();
    }
}