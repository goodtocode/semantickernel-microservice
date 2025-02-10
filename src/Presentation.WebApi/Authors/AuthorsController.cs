using Goodtocode.SemanticKernel.Core.Application.Author;
using Goodtocode.SemanticKernel.Core.Application.ChatCompletion;
using Goodtocode.SemanticKernel.Core.Application.Common.Models;
using Goodtocode.SemanticKernel.Presentation.WebApi.Common;

namespace Goodtocode.SemanticKernel.Presentation.WebApi.Authors;

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

    /// <summary>Get All Author Chat Sessions Query</summary>
    /// <remarks>
    /// Sample request:
    ///
    ///     "AuthorId": 60fb5e99-3a78-43df-a512-7d8ff498499e
    ///     "StartDate": "2024-06-01T00:00:00Z"
    ///     "EndDate": "2024-12-01T00:00:00Z"
    ///     "api-version":  1.0
    /// 
    /// </remarks>
    /// <returns>
    /// ChatSessionDto
    ///     { Id: 1efb5e99-3a78-43df-a512-7d8ff498499e
    ///     AuthorId: 4dfb5e99-3a78-43df-a512-7d8ff498499e
    ///     Timestamp: "2024-06-03T11:21:00Z"
    ///     Messages: [
    ///         {
    ///             "Id": 60fb5e99-3a78-43df-a512-7d8ff498499e,
    ///             "Content": "Certainly! Semantic Kernel is a great framework for AI.",
    ///         }
    ///     }]
    /// </returns>
    [HttpGet("{id}/ChatSessions", Name = "GetAuthorChatSessionsQuery")]
    [ProducesResponseType(typeof(ICollection<ChatSessionDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ICollection<ChatSessionDto>> GetAuthorChatSessionsQuery(Guid id)
    {
        return await Mediator.Send(new GetAuthorChatSessionsQuery() { AuthorId = id });
    }

    /// <summary>Get Author Chat Sessions Paginated Query</summary>
    /// <remarks>
    /// Sample request:
    ///
    ///     "StartDate": "2024-06-01T00:00:00Z"
    ///     "EndDate": "2024-12-01T00:00:00Z"
    ///     "PageNumber": 1
    ///     "PageSize" : 10
    ///     "api-version":  1.0
    /// 
    /// </remarks>
    /// <returns>
    /// ChatSessionDto
    ///     { Id: 1efb5e99-3a78-43df-a512-7d8ff498499e
    ///     AuthorId: 4dfb5e99-3a78-43df-a512-7d8ff498499e
    ///     Messages: [
    ///         {
    ///             "Id": 60fb5e99-3a78-43df-a512-7d8ff498499e,
    ///             "Content": "Certainly! Semantic Kernel is a great framework for AI.",
    ///         }
    ///     }]
    /// </returns>
    [HttpGet("{id}/ChatSessions/Paginated", Name = "GetAuthorChatSessionsPaginatedQuery")]
    [ProducesResponseType(typeof(PaginatedList<ChatSessionDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<PaginatedList<ChatSessionDto>>> GetAuthorChatSessionsPaginatedQuery(Guid id, DateTime? startDate, DateTime? endDate, int pageNumber = 1, int pageSize = 10)
    {
        var query = new GetAuthorChatSessionsPaginatedQuery()
        {
            AuthorId = id,
            StartDate = startDate,
            EndDate = endDate,
            PageNumber = pageNumber,
            PageSize = pageSize

        };
        return await Mediator.Send(query);
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