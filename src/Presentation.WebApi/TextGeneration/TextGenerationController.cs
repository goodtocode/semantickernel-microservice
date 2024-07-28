using Goodtocode.SemanticKernel.Core.Application.Common.Models;
using Goodtocode.SemanticKernel.Core.Application.TextGeneration;
using Goodtocode.SemanticKernel.Presentation.WebApi.Common;

namespace Goodtocode.SemanticKernel.Presentation.WebApi.TextGeneration;

/// <summary>
/// Text Generation endpoints to create a chat, continue a chat, delete a chat and retrieve chat history
/// </summary>
[ApiController]
[ApiConventionType(typeof(DefaultApiConventions))]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
public class TextGenerationController : ApiControllerBase
{
    /// <summary>Get Text Generation session with history</summary>
    /// <remarks>
    /// Sample request:
    ///
    ///        "Id": 60fb5e99-3a78-43df-a512-7d8ff498499e
    ///        "api-version":  1.0
    /// 
    /// </remarks>
    /// <returns>
    /// TextPromptDto
    ///     { Id: 1efb5e99-3a78-43df-a512-7d8ff498499e
    ///     AuthorKey: 4dfb5e99-3a78-43df-a512-7d8ff498499e
    ///     Messages: [
    ///         {
    ///             "Id": 60fb5e99-3a78-43df-a512-7d8ff498499e,
    ///             "Content": "Certainly! Semantic Kernel is a great framework for AI.",
    ///         }
    ///     }]
    /// </returns>
    [HttpGet("{id}", Name = "GetTextPromptQuery")]
    [ProducesResponseType(typeof(TextPromptDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<TextPromptDto> Get(Guid id)
    {
        return await Mediator.Send(new GetTextPromptQuery
        {
            Id = id
        });
    }

    /// <summary>Get All Text Generations Query</summary>
    /// <remarks>
    /// Sample request:
    ///
    ///     "StartDate": "2024-06-01T00:00:00Z"
    ///     "EndDate": "2024-12-01T00:00:00Z"
    ///     "api-version":  1.0
    /// 
    /// </remarks>
    /// <returns>
    /// TextPromptDto
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
    [HttpGet(Name = "GetTextPromptsQuery")]
    [ProducesResponseType(typeof(ICollection<TextPromptDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ICollection<TextPromptDto>> GetAll()
    {
        return await Mediator.Send(new GetTextPromptsQuery());
    }

    /// <summary>Get Text Generations Paginated Query</summary>
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
    /// TextPromptDto
    ///     { Id: 1efb5e99-3a78-43df-a512-7d8ff498499e
    ///     AuthorId: 4dfb5e99-3a78-43df-a512-7d8ff498499e
    ///     Messages: [
    ///         {
    ///             "Id": 60fb5e99-3a78-43df-a512-7d8ff498499e,
    ///             "Content": "Certainly! Semantic Kernel is a great framework for AI.",
    ///         }
    ///     }]
    /// </returns>
    [HttpGet("Paginated", Name = "GetTextPromptsPaginatedQuery")]
    [ProducesResponseType(typeof(PaginatedList<TextPromptDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<PaginatedList<TextPromptDto>>> GetTextPromptsPaginatedQuery([FromQuery] GetTextPromptsPaginatedQuery query)
    {
        return await Mediator.Send(query);
    }

    /// <summary>
    /// Creates new Text Generation session
    /// </summary>
    /// <remarks>
    /// Types of Text Generation are:
    ///     1. Generic Prompt: A broad or open-ended request for content.
    ///         - Example Prompt: “Write a short story.”
    ///         - Example Response: “Once upon a time, in a quaint village, there lived a curious cat named Whiskers…”
    ///     2. Specific Prompt: A prompt with clear instructions or a specific topic.
    ///         - Example Prompt: “Describe the process of photosynthesis.”
    ///         - Example Response: “Photosynthesis is the process by which plants convert sunlight into energy, using chlorophyll in their leaves…”
    ///     3. Visual Prompt: A prompt related to an image or visual content.
    ///         - Example Prompt: “Describe this image: ‘A serene sunset over the ocean.’”
    ///         - Example Response: “The sun dipped below the horizon, casting hues of orange and pink across the calm waters…”
    ///     4. Role-Based Prompt: A prompt where you assume a specific role or context.
    ///         - Example Prompt: “Act as a travel guide.Describe the beauty of the Swiss Alps.”
    ///         - Example Response: “Welcome to the majestic Swiss Alps! Snow-capped peaks, pristine lakes, and charming villages await…”
    ///     5. Output Format Specification: A prompt specifying the desired output format.
    ///         - Example Prompt: “Summarize the key findings of the research paper.”
    ///         - Example Response: “The paper discusses novel algorithms for optimizing neural network training, achieving faster convergence…”
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
    /// TextPromptDto
    ///     { Id: 1efb5e99-3a78-43df-a512-7d8ff498499e
    ///     AuthorId: 4dfb5e99-3a78-43df-a512-7d8ff498499e
    ///     Messages: [
    ///         {
    ///             "Id": 60fb5e99-3a78-43df-a512-7d8ff498499e,
    ///             "Content": "Certainly! Semantic Kernel is a great framework for AI.",
    ///         }
    ///     }]
    /// </returns>
    [HttpPost(Name = "CreateTextPromptCommand")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> Post(CreateTextPromptCommand command)
    {
        var response = await Mediator.Send(command);
        return CreatedAtAction(nameof(Get), new { response.Id }, response);
    }

    /// <summary>Remove TextGeneration Command</summary>
    /// <remarks>
    /// Sample request:
    ///
    ///        "Id": 60fb5e99-3a78-43df-a512-7d8ff498499e
    ///        "api-version":  1.0
    /// 
    /// </remarks>
    /// <returns>NoContent</returns>
    [HttpDelete("{id}", Name = "RemoveTextPromptCommand")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesDefaultResponseType]
    public async Task<IActionResult> Delete(Guid id)
    {
        await Mediator.Send(new DeleteTextPromptCommand() { Id = id });

        return NoContent();
    }
}