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
    ///        "Id": 60fb5e99-3a78-43df-a512-7d8ff498499e
    ///        "api-version":  1.0
    /// 
    /// </remarks>
    /// <returns>
    /// AuthorDto
    ///     { 
    ///         Id: 1efb5e99-3a78-43df-a512-7d8ff498499e
    ///         Name: John Doe
    ///     }
    /// </returns>
    [HttpGet("{id}", Name = "GetAuthorQuery")]
    [ProducesResponseType(typeof(AuthorDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<AuthorDto> Get(Guid id)
    {
        return await Mediator.Send(new GetAuthorQuery
        {
            AuthorId = id
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
    ///        "Id": 00000000-0000-0000-0000-000000000000,
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
    ///        "Id": "60fb5e99-3a78-43df-a512-7d8ff498499e",
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
    ///        "Id": 60fb5e99-3a78-43df-a512-7d8ff498499e
    ///        "api-version":  1.0
    /// 
    /// </remarks>
    /// <returns>NoContent</returns>
    [HttpDelete("{id}", Name = "RemoveAuthorCommand")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesDefaultResponseType]
    public async Task<IActionResult> Delete(Guid id)
    {
        await Mediator.Send(new DeleteAuthorCommand() { Id = id });

        return NoContent();
    }
}