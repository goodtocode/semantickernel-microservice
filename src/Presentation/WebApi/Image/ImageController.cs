using Goodtocode.SemanticKernel.Core.Application.Common.Models;
using Goodtocode.SemanticKernel.Core.Application.Image;
using Goodtocode.SemanticKernel.Presentation.WebApi.Common;

namespace Goodtocode.SemanticKernel.Presentation.WebApi.Image;

/// <summary>
/// Text Image endpoints to create a chat, continue a chat, delete a chat and retrieve chat history
/// </summary>
[ApiController]
[ApiConventionType(typeof(DefaultApiConventions))]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
public class ImageController : ApiControllerBase
{
    /// <summary>Get Text Image with history</summary>
    /// <remarks>
    /// Sample request:
    ///
    ///        "Id": 60fb5e99-3a78-43df-a512-7d8ff498499e
    ///        "api-version":  1.0
    /// 
    /// </remarks>
    /// <returns>
    /// TextImageDto
    ///     { Id: 1efb5e99-3a78-43df-a512-7d8ff498499e
    ///     AuthorKey: 4dfb5e99-3a78-43df-a512-7d8ff498499e
    ///     Messages: [
    ///         {
    ///             "Id": 60fb5e99-3a78-43df-a512-7d8ff498499e,
    ///             "Content": "Certainly! Semantic Kernel is a great framework for AI.",
    ///         }
    ///     }]
    /// </returns>
    [HttpGet("{id}", Name = "GetTextImageQuery")]
    [ProducesResponseType(typeof(TextImageDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<TextImageDto> Get(Guid id)
    {
        return await Mediator.Send(new GetTextImageQuery
        {
            Id = id
        });
    }

    /// <summary>Get All Text Images Query</summary>
    /// <remarks>
    /// Sample request:
    ///
    ///     "StartDate": "2024-06-01T00:00:00Z"
    ///     "EndDate": "2024-12-01T00:00:00Z"
    ///     "api-version":  1.0
    /// 
    /// </remarks>
    /// <returns>
    /// TextImageDto
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
    [HttpGet(Name = "GetTextImagesQuery")]
    [ProducesResponseType(typeof(ICollection<TextImageDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ICollection<TextImageDto>> GetAll()
    {
        return await Mediator.Send(new GetTextImagesQuery());
    }

    /// <summary>Get Text Images Paginated Query</summary>
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
    /// TextImageDto
    ///     { Id: 1efb5e99-3a78-43df-a512-7d8ff498499e
    ///     AuthorId: 4dfb5e99-3a78-43df-a512-7d8ff498499e
    ///     Messages: [
    ///         {
    ///             "Id": 60fb5e99-3a78-43df-a512-7d8ff498499e,
    ///             "Content": "Certainly! Semantic Kernel is a great framework for AI.",
    ///         }
    ///     }]
    /// </returns>
    [HttpGet("Paginated", Name = "GetTextImagesPaginatedQuery")]
    [ProducesResponseType(typeof(PaginatedList<TextImageDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<PaginatedList<TextImageDto>>> GetTextImagesPaginatedQuery([FromQuery] GetTextImagesPaginatedQuery query)
    {
        return await Mediator.Send(query);
    }

    /// <summary>
    /// Creates new Image from Text
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
    /// TextImageDto
    ///     { Id: 1efb5e99-3a78-43df-a512-7d8ff498499e
    ///     AuthorId: 4dfb5e99-3a78-43df-a512-7d8ff498499e
    ///     Messages: [
    ///         {
    ///             "Id": 60fb5e99-3a78-43df-a512-7d8ff498499e,
    ///             "Content": "Certainly! Semantic Kernel is a great framework for AI.",
    ///         }
    ///     }]
    /// </returns>
    [HttpPost("TextToImage", Name = "CreateTextToImageCommand")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> Post(CreateTextToImageCommand command)
    {
        var response = await Mediator.Send(command);
        return CreatedAtAction(nameof(Get), new { response.Id }, response);
    }

    /// <summary>
    /// Creates new Text from Image
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
    /// TextImageDto
    ///     { Id: 1efb5e99-3a78-43df-a512-7d8ff498499e
    ///     AuthorId: 4dfb5e99-3a78-43df-a512-7d8ff498499e
    ///     Messages: [
    ///         {
    ///             "Id": 60fb5e99-3a78-43df-a512-7d8ff498499e,
    ///             "Content": "Certainly! Semantic Kernel is a great framework for AI.",
    ///         }
    ///     }]
    /// </returns>
    [HttpPost("ImageToText", Name = "CreateImageToTextCommand")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> Post(CreateImageToTextCommand command)
    {
        var response = await Mediator.Send(command);
        return CreatedAtAction(nameof(Get), new { response.Id }, response);
    }

    /// <summary>Remove Image Command</summary>
    /// <remarks>
    /// Sample request:
    ///
    ///        "Id": 60fb5e99-3a78-43df-a512-7d8ff498499e
    ///        "api-version":  1.0
    /// 
    /// </remarks>
    /// <returns>NoContent</returns>
    [HttpDelete("{id}", Name = "RemoveTextImageCommand")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesDefaultResponseType]
    public async Task<IActionResult> Delete(Guid id)
    {
        await Mediator.Send(new DeleteTextImageCommand() { Id = id });

        return NoContent();
    }
}