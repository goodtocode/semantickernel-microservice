using Goodtocode.SemanticKernel.Core.Application.ChatCompletion;
using Goodtocode.SemanticKernel.Core.Application.Common.Models;
using Goodtocode.SemanticKernel.Presentation.WebApi.Common;

namespace Goodtocode.SemanticKernel.Presentation.WebApi.ChatCompletion;

/// <summary>
/// Chat completion endpoints to create a chat, continue a chat, delete a chat and retrieve chat history
/// </summary>
[ApiController]
[ApiConventionType(typeof(DefaultApiConventions))]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
public class ChatMessageController : ApiControllerBase
{
    /// <summary>Get Chat Message</summary>
    /// <remarks>
    /// Sample request:
    ///
    ///        "Id": 60fb5e99-3a78-43df-a512-7d8ff498499e
    ///        "api-version":  1.0
    /// 
    /// </remarks>
    /// <returns>
    /// ChatMessageDto
    ///     { 
    ///         "Id": 60fb5e99-3a78-43df-a512-7d8ff498499e,
    ///         "Content": "Certainly! Semantic Kernel is a great framework for AI."
    ///     }
    /// </returns>
    [HttpGet("{id}", Name = "GetChatMessageQuery")]
    [ProducesResponseType(typeof(ChatMessageDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ChatMessageDto> Get(Guid id)
    {
        return await Mediator.Send(new GetChatMessageQuery
        {
            Id = id
        });
    }

    /// <summary>Get All Chat Messages for a session Query</summary>
    /// <remarks>
    /// Sample request:
    ///
    ///     "StartDate": "2024-06-01T00:00:00Z"
    ///     "EndDate": "2024-12-01T00:00:00Z"
    ///     "api-version":  1.0
    /// 
    /// </remarks>
    /// <returns>
    /// ChatMessageDto
    ///     [{
    ///         "Id": 60fb5e99-3a78-43df-a512-7d8ff498499e,
    ///         "Content": "Certainly! Semantic Kernel is a great framework for AI.",
    ///     }]
    /// </returns>
    [HttpGet(Name = "GetChatMessagesQuery")]
    [ProducesResponseType(typeof(ICollection<ChatMessageDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ICollection<ChatMessageDto>> GetAll()
    {
        return await Mediator.Send(new GetChatMessagesQuery());
    }

    /// <summary>Get Chat Messages Paginated Query</summary>
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
    /// ChatMessageDto
    ///     { Id: 1efb5e99-3a78-43df-a512-7d8ff498499e
    ///     AuthorId: 4dfb5e99-3a78-43df-a512-7d8ff498499e
    ///     Messages: [
    ///         {
    ///             "Id": 60fb5e99-3a78-43df-a512-7d8ff498499e,
    ///             "Content": "Certainly! Semantic Kernel is a great framework for AI.",
    ///         }
    ///     }]
    /// </returns>
    [HttpGet("Paginated", Name = "GetChatMessagesPaginatedQuery")]
    [ProducesResponseType(typeof(PaginatedList<ChatMessageDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<PaginatedList<ChatMessageDto>>> GetPaginated([FromQuery] GetChatMessagesPaginatedQuery query)
    {
        return await Mediator.Send(query);
    }

    /// <summary>
    /// Creates new Chat Message with initial message prompt/response history
    /// </summary>
    /// <remarks>
    /// Types of Chat Completion are:
    ///     1. Informational Prompt: A prompt requesting information
    ///         - Example Prompt: "What's the capital of France?"
    ///         - Example Response: "The capital of France is Paris."
    ///     2. Multiple Choice Prompt: A prompt with instructions for multiple-choice responses.
    ///         - Example Prompt: “Choose an activity for the weekend: a) Hiking b) Movie night c) Cooking class d) Board games”
    ///         - Example Response: “I'd recommend hiking! It's a great way to enjoy nature and get some exercise.”
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
    /// ChatMessageDto
    ///     { Id: 1efb5e99-3a78-43df-a512-7d8ff498499e
    ///     AuthorId: 4dfb5e99-3a78-43df-a512-7d8ff498499e
    ///     Messages: [
    ///         {
    ///             "Id": 60fb5e99-3a78-43df-a512-7d8ff498499e,
    ///             "Content": "Certainly! Semantic Kernel is a great framework for AI.",
    ///         }
    ///     }]
    /// </returns>
    [HttpPost(Name = "CreateChatMessageCommand")]
    [ProducesResponseType<ChatMessageDto>(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> Post(CreateChatMessageCommand command)
    {
        var response = await Mediator.Send(command);
        return CreatedAtAction(nameof(Get), new { response.Id }, response);
    }
}