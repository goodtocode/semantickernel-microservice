using Goodtocode.SemanticKernel.Core.Application.ChatCompletion;
using Goodtocode.SemanticKernel.Presentation.WebApi.Common;

namespace Goodtocode.SemanticKernel.Presentation.WebApi.ChatSession;

/// <summary>
/// Chat completion endpoints to create a chat, continue a chat, delete a chat and retrieve chat history
/// </summary>
[ApiController]
[ApiConventionType(typeof(DefaultApiConventions))]
[Route("[controller]")]
[ApiVersion("1.0")]
public class ChatSessionController : ApiControllerBase
{
    /// <summary>Get Chat Completion session with history</summary>
    /// <remarks>
    /// Sample request:
    ///
    ///        "Key": 60fb5e99-3a78-43df-a512-7d8ff498499e
    ///        "api-version":  1.0
    /// 
    /// </remarks>
    /// <returns>ChatSessionDto</returns>
    [HttpGet("{key}", Name = "GetChatSessionQuery")]
    [ProducesResponseType(typeof(ChatSessionDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ChatSessionDto> Get(Guid key)
    {
        return await Mediator.Send(new GetChatSessionQuery
        {
            Key = key
        });
    }

    /// <summary>
    /// Creates new Chat Completion session with empty history
    /// </summary>
    /// <remarks>
    /// Sample request:
    ///
    ///     HttpPost Body
    ///     {
    ///        "Key": "00000000-0000-0000-0000-000000000000",
    ///        "Message":  "Hi, I am interested in learning about Semantic Kernel."
    ///     }
    ///
    ///     "version":  1.0
    /// </remarks>
    /// <param name="command"></param>
    /// <returns>    
    ///     {
    ///        "Key": "00000000-0000-0000-0000-000000000000",
    ///        "Message":  "Hi, I am interested in learning about Semantic Kernel.",
    ///        "Response": "Certainly! Semantic Kernel is a great framework for AI.",
    ///     }
    /// </returns>
    [HttpPost(Name = "CreateChatSessionCommand")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> Post(CreateChatSessionCommand command)
    {
        var response = await Mediator.Send(command);
        return CreatedAtAction(nameof(Get), new { response.Key }, response);
    }

    ///// <summary>
    ///// Update ChatSession Command
    ///// </summary>
    ///// <remarks>
    ///// Sample request:
    /////
    /////     HttpPut Body
    /////     {
    /////        "Key": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    /////        "Message":  "Hi, I am interested in learning about Semantic Kernel.",
    /////        "Content": "Certainly! Semantic Kernel is a great framework for AI.",
    /////     }
    /////
    /////     "version":  1.0
    ///// </remarks>
    ///// <param name="command"></param>
    ///// <returns>    
    /////     {
    /////        "Key": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    /////        "Message":  "Hi, I am interested in learning about Semantic Kernel.",
    /////        "Content": "Certainly! Semantic Kernel is a great framework for AI.",
    /////     }</returns>
    //[HttpPut(Name = "UpdateChatSessionCommand")]
    //[ProducesResponseType(StatusCodes.Status204NoContent)]
    //[ProducesResponseType(StatusCodes.Status404NotFound)]
    //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
    //public async Task<ActionResult> Put(UpdateChatSessionCommand command)
    //{
    //    await Mediator.Send(command);

    //    return NoContent();
    //}

    ///// <summary>
    ///// Patch ChatSession Command
    ///// </summary>
    ///// <remarks>
    ///// Sample request:
    /////
    /////     HttpPatch Body
    /////     {
    /////        "Key": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    /////        "Date": "2023-06-08T23:32:05.256Z",
    /////        "TemperatureC": 0,
    /////        "Zipcodes": [ 92602, 92673 ]
    /////     }
    /////
    /////     "version":  1.0
    ///// </remarks>
    ///// <param name="command"></param>
    ///// <returns>NoContent</returns>
    //[HttpPatch(Name = "PatchChatSessionCommand")]
    //[ProducesResponseType(StatusCodes.Status204NoContent)]
    //[ProducesResponseType(StatusCodes.Status404NotFound)]
    //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
    //public async Task<ActionResult> Patch(PatchChatSessionCommand command)
    //{
    //    await Mediator.Send(command);

    //    return NoContent();
    //}

    ///// <summary>Remove ChatSession Command</summary>
    ///// <remarks>
    ///// Sample request:
    /////
    /////        "Key": 60fb5e99-3a78-43df-a512-7d8ff498499e
    /////        "api-version":  1.0
    ///// 
    ///// </remarks>
    ///// <returns>NoContent</returns>
    //[HttpDelete("{key}", Name = "RemoveChatSessionCommand")]
    //[ProducesResponseType(StatusCodes.Status204NoContent)]
    //[ProducesResponseType(StatusCodes.Status404NotFound)]
    //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
    //[ProducesDefaultResponseType]
    //public async Task<IActionResult> Delete(Guid key)
    //{
    //    await Mediator.Send(new RemoveChatSessionCommand() { Key = key });

    //    return NoContent();
    //}
}