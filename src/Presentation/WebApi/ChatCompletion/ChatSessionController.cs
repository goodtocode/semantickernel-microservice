using Goodtocode.SemanticKernel.Core.Application.ChatCompletion;
using Goodtocode.SemanticKernel.Core.Application.Common.Models;
using Goodtocode.SemanticKernel.Presentation.WebApi.Common;

namespace Goodtocode.SemanticKernel.Presentation.WebApi.ChatSession;

/// <summary>
/// Chat completion endpoints to create a chat, continue a chat, delete a chat and retrieve chat history
/// </summary>
[ApiController]
[ApiConventionType(typeof(DefaultApiConventions))]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
public class ChatSessionController : ApiControllerBase
{
    /// <summary>Get Chat Completion session with history</summary>
    /// <remarks>
    /// Sample request:
    ///
    ///        "Id": 60fb5e99-3a78-43df-a512-7d8ff498499e
    ///        "api-version":  1.0
    /// 
    /// </remarks>
    /// <returns>
    /// ChatSessionDto
    ///     { Id: 1efb5e99-3a78-43df-a512-7d8ff498499e
    ///     AuthorKey: 4dfb5e99-3a78-43df-a512-7d8ff498499e
    ///     Messages: [
    ///         {
    ///             "Id": 60fb5e99-3a78-43df-a512-7d8ff498499e,
    ///             "Content": "Certainly! Semantic Kernel is a great framework for AI.",
    ///         }
    ///     }]
    /// </returns>
    [HttpGet("{id}", Name = "GetChatSessionQuery")]
    [ProducesResponseType(typeof(ChatSessionDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ChatSessionDto> Get(Guid id)
    {
        return await Mediator.Send(new GetChatSessionQuery
        {
            Id = id
        });
    }

    /// <summary>Get All Chat Sessions Query</summary>
    /// <remarks>
    /// Sample request:
    ///
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
    [HttpGet(Name = "GetChatSessionsQuery")]
    [ProducesResponseType(typeof(ICollection<ChatSessionDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ICollection<ChatSessionDto>> GetAll()
    {
        return await Mediator.Send(new GetChatSessionsQuery());
    }

    /// <summary>Get Chat Sessions Paginated Query</summary>
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
    [HttpGet("Paginated", Name = "GetChatSessionsPaginatedQuery")]
    [ProducesResponseType(typeof(PaginatedList<ChatSessionDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<PaginatedList<ChatSessionDto>>> GetSemanticKernelMicroservicePaginatedQuery([FromQuery] GetChatSessionsPaginatedQuery query)
    {
        return await Mediator.Send(query);
    }

    /// <summary>
    /// Creates new Chat Completion session with empty history
    /// </summary>
    /// <remarks>
    /// Sample request:
    ///
    ///     HttpPost Body
    ///     {
    ///        "Id": 00000000-0000-0000-0000-000000000000,
    ///        "Message":  "Hi, I am interested in learning about Semantic Kernel."
    ///     }
    ///
    ///     "version":  1.0
    /// </remarks>
    /// <param name="command"></param>
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
    [HttpPost(Name = "CreateChatSessionCommand")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> Post(CreateChatSessionCommand command)
    {
        var response = await Mediator.Send(command);
        return CreatedAtAction(nameof(Get), new { response.Id }, response);
    }

    /// <summary>
    /// Update ChatSession Command
    /// </summary>
    /// <remarks>
    /// Sample request:
    ///
    ///     HttpPut Body
    ///     {
    ///        "Id": "60fb5e99-3a78-43df-a512-7d8ff498499e",
    ///        "Message":  "Hi, I am interested in learning about Semantic Kernel.",
    ///        "Content": "Certainly! Semantic Kernel is a great framework for AI.",
    ///     }
    ///
    ///     "version":  1.0
    /// </remarks>
    /// <param name="command"></param>
    /// <returns>    
    ///     {
    ///        "Id": "60fb5e99-3a78-43df-a512-7d8ff498499e",
    ///        "Message":  "Hi, I am interested in learning about Semantic Kernel.",
    ///        "Content": "Certainly! Semantic Kernel is a great framework for AI.",
    ///     }</returns>
    [HttpPut(Name = "UpdateChatSessionCommand")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> Put(UpdateChatSessionCommand command)
    {
        await Mediator.Send(command);

        return NoContent();
    }

    /// <summary>
    /// Patch Chat Session Command
    /// </summary>
    /// <remarks>
    /// Sample request:
    ///
    ///     HttpPatch Body
    ///     {
    ///        "Id": "60fb5e99-3a78-43df-a512-7d8ff498499e",
    ///        "Title":  "Semantic Kernel Chat Session"
    ///     }
    ///
    ///     "version":  1.0
    /// </remarks>
    /// <param name="id"></param>
    /// <param name="command"></param>
    /// <returns>NoContent</returns>
    [HttpPatch("{id}", Name = "PatchChatSessionCommand")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> Patch(Guid id, PatchChatSessionCommand command)
    {
        command.Id = id;
        await Mediator.Send(command);

        return NoContent();
    }

    /// <summary>Remove ChatSession Command</summary>
    /// <remarks>
    /// Sample request:
    ///
    ///        "Id": 60fb5e99-3a78-43df-a512-7d8ff498499e
    ///        "api-version":  1.0
    /// 
    /// </remarks>
    /// <returns>NoContent</returns>
    [HttpDelete("{id}", Name = "RemoveChatSessionCommand")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesDefaultResponseType]
    public async Task<IActionResult> Delete(Guid id)
    {
        await Mediator.Send(new DeleteChatSessionCommand() { Id = id });

        return NoContent();
    }
}