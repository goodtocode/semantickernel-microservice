using Goodtocode.SemanticKernel.Core.Application.Common.Models;
using Goodtocode.SemanticKernel.Core.Application.Audio;
using Goodtocode.SemanticKernel.Presentation.WebApi.Common;

namespace Goodtocode.SemanticKernel.Presentation.WebApi.Audio;

/// <summary>
/// Text Audio endpoints to create a chat, continue a chat, delete a chat and retrieve chat history
/// </summary>
[ApiController]
[ApiConventionType(typeof(DefaultApiConventions))]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
public class AudioController : ApiControllerBase
{
    /// <summary>Get Text Audio with history</summary>
    /// <remarks>
    /// Sample request:
    ///
    ///        "Id": 60fb5e99-3a78-43df-a512-7d8ff498499e
    ///        "api-version":  1.0
    /// 
    /// </remarks>
    /// <returns>
    /// TextAudioDto
    ///     { Id: 1efb5e99-3a78-43df-a512-7d8ff498499e
    ///     AuthorKey: 4dfb5e99-3a78-43df-a512-7d8ff498499e
    ///     Messages: [
    ///         {
    ///             "Id": 60fb5e99-3a78-43df-a512-7d8ff498499e,
    ///             "Content": "Certainly! Semantic Kernel is a great framework for AI.",
    ///         }
    ///     }]
    /// </returns>
    [HttpGet("{id}", Name = "GetTextAudioQuery")]
    [ProducesResponseType(typeof(TextAudioDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<TextAudioDto> Get(Guid id)
    {
        return await Mediator.Send(new GetTextAudioQuery
        {
            Id = id
        });
    }

    /// <summary>Get All Text Audios Query</summary>
    /// <remarks>
    /// Sample request:
    ///
    ///     "StartDate": "2024-06-01T00:00:00Z"
    ///     "EndDate": "2024-12-01T00:00:00Z"
    ///     "api-version":  1.0
    /// 
    /// </remarks>
    /// <returns>
    /// TextAudioDto
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
    [HttpGet(Name = "GetTextAudiosQuery")]
    [ProducesResponseType(typeof(ICollection<TextAudioDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ICollection<TextAudioDto>> GetAll()
    {
        return await Mediator.Send(new GetTextAudiosQuery());
    }

    /// <summary>Get Text Audios Paginated Query</summary>
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
    /// TextAudioDto
    ///     { Id: 1efb5e99-3a78-43df-a512-7d8ff498499e
    ///     AuthorId: 4dfb5e99-3a78-43df-a512-7d8ff498499e
    ///     Messages: [
    ///         {
    ///             "Id": 60fb5e99-3a78-43df-a512-7d8ff498499e,
    ///             "Content": "Certainly! Semantic Kernel is a great framework for AI.",
    ///         }
    ///     }]
    /// </returns>
    [HttpGet("Paginated", Name = "GetTextAudioPaginatedQuery")]
    [ProducesResponseType(typeof(PaginatedList<TextAudioDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<PaginatedList<TextAudioDto>>> GetTextAudioPaginatedQuery([FromQuery] GetTextAudioPaginatedQuery query)
    {
        return await Mediator.Send(query);
    }

    /// <summary>
    /// Creates new Audio from Text
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
    /// TextAudioDto
    ///     { Id: 1efb5e99-3a78-43df-a512-7d8ff498499e
    ///     AuthorId: 4dfb5e99-3a78-43df-a512-7d8ff498499e
    ///     Messages: [
    ///         {
    ///             "Id": 60fb5e99-3a78-43df-a512-7d8ff498499e,
    ///             "Content": "Certainly! Semantic Kernel is a great framework for AI.",
    ///         }
    ///     }]
    /// </returns>
    [HttpPost(Name = "CreateTextToAudioCommand")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> Post(CreateTextToAudioCommand command)
    {
        var response = await Mediator.Send(command);
        return CreatedAtAction(nameof(Get), new { response.Id }, response);
    }

    /// <summary>Remove Audio Command</summary>
    /// <remarks>
    /// Sample request:
    ///
    ///        "Id": 60fb5e99-3a78-43df-a512-7d8ff498499e
    ///        "api-version":  1.0
    /// 
    /// </remarks>
    /// <returns>NoContent</returns>
    [HttpDelete("{id}", Name = "RemoveTextAudioCommand")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesDefaultResponseType]
    public async Task<IActionResult> Delete(Guid id)
    {
        await Mediator.Send(new DeleteTextAudioCommand() { Id = id });

        return NoContent();
    }
}