//using Goodtocode.SemanticKernel.Presentation.WebApi.Common;

//namespace Goodtocode.SemanticKernel.Presentation.WebApi.Authors;

///// <summary>
///// Author endpoints to create a chat, continue a chat, delete a chat and retrieve chat history
///// </summary>
//[ApiController]
//[ApiConventionType(typeof(DefaultApiConventions))]
//[Route("api/v{version:apiVersion}/[controller]")]
//[ApiVersion("1.0")]
//public class AuthorsController : ApiControllerBase
//{
//    /// <summary>Get Author session with history</summary>
//    /// <remarks>
//    /// Sample request:
//    ///
//    ///        "Key": 60fb5e99-3a78-43df-a512-7d8ff498499e
//    ///        "api-version":  1.0
//    /// 
//    /// </remarks>
//    /// <returns>
//    /// AuthorsDto
//    ///     { Key: 1efb5e99-3a78-43df-a512-7d8ff498499e
//    ///     AuthorKey: 4dfb5e99-3a78-43df-a512-7d8ff498499e
//    ///     Messages: [
//    ///         {
//    ///             "Key": 60fb5e99-3a78-43df-a512-7d8ff498499e,
//    ///             "Content": "Certainly! Semantic Kernel is a great framework for AI.",
//    ///         }
//    ///     }]
//    /// </returns>
//    [HttpGet("{key}", Name = "GetAuthorsQuery")]
//    [ProducesResponseType(typeof(AuthorsDto), StatusCodes.Status200OK)]
//    [ProducesResponseType(StatusCodes.Status404NotFound)]
//    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
//    public async Task<AuthorsDto> Get(Guid key)
//    {
//        return await Mediator.Send(new GetAuthorsQuery
//        {
//            Key = key
//        });
//    }

//    /// <summary>Get Paginated Query</summary>
//    /// <remarks>
//    /// Sample request:
//    ///
//    ///        "PageNumber": 1
//    ///        "PageSize" : 10
//    ///        "api-version":  1.0
//    /// 
//    /// </remarks>
//    /// <returns>
//    /// AuthorsDto
//    ///     { Key: 1efb5e99-3a78-43df-a512-7d8ff498499e
//    ///     AuthorKey: 4dfb5e99-3a78-43df-a512-7d8ff498499e
//    ///     Messages: [
//    ///         {
//    ///             "Key": 60fb5e99-3a78-43df-a512-7d8ff498499e,
//    ///             "Content": "Certainly! Semantic Kernel is a great framework for AI.",
//    ///         }
//    ///     }]
//    /// </returns>
//    [HttpGet("Paginated", Name = "GetAuthorssPaginatedQuery")]
//    [ProducesResponseType(typeof(PaginatedList<ForecastPaginatedDto>), StatusCodes.Status200OK)]
//    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
//    public async Task<ActionResult<PaginatedList<ForecastPaginatedDto>>> GetSemanticKernelMicroservicePaginatedQuery([FromQuery] GetForecastsPaginatedQuery query)
//    {
//        return await Mediator.Send(query);
//    }

//    /// <summary>Get All Chat Sessions Query</summary>
//    /// <remarks>
//    /// Sample request:
//    ///
//    ///     "StartDate": "2024-06-01T00:00:00Z"
//    ///     "EndDate": "2024-12-01T00:00:00Z"
//    ///     "api-version":  1.0
//    /// 
//    /// </remarks>
//    /// <returns>
//    /// AuthorsDto
//    ///     { Key: 1efb5e99-3a78-43df-a512-7d8ff498499e
//    ///     AuthorKey: 4dfb5e99-3a78-43df-a512-7d8ff498499e
//    ///     Timestamp: "2024-06-03T11:21:00Z"
//    ///     Messages: [
//    ///         {
//    ///             "Key": 60fb5e99-3a78-43df-a512-7d8ff498499e,
//    ///             "Content": "Certainly! Semantic Kernel is a great framework for AI.",
//    ///         }
//    ///     }]
//    /// </returns>
//    [HttpGet(Name = "GetAuthorssQuery")]
//    [ProducesResponseType(typeof(ICollection<AuthorsDto>), StatusCodes.Status200OK)]
//    [ProducesResponseType(StatusCodes.Status404NotFound)]
//    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
//    public async Task<ICollection<AuthorsDto>> GetAll()
//    {
//        return await Mediator.Send(new GetAuthorssQuery());
//    }

//    /// <summary>
//    /// Creates new Author session with empty history
//    /// </summary>
//    /// <remarks>
//    /// Sample request:
//    ///
//    ///     HttpPost Body
//    ///     {
//    ///        "Key": 00000000-0000-0000-0000-000000000000,
//    ///        "Message":  "Hi, I am interested in learning about Semantic Kernel."
//    ///     }
//    ///
//    ///     "version":  1.0
//    /// </remarks>
//    /// <param name="command"></param>
//    /// <returns>
//    /// AuthorsDto
//    ///     { Key: 1efb5e99-3a78-43df-a512-7d8ff498499e
//    ///     AuthorKey: 4dfb5e99-3a78-43df-a512-7d8ff498499e
//    ///     Messages: [
//    ///         {
//    ///             "Key": 60fb5e99-3a78-43df-a512-7d8ff498499e,
//    ///             "Content": "Certainly! Semantic Kernel is a great framework for AI.",
//    ///         }
//    ///     }]
//    /// </returns>
//    [HttpPost(Name = "CreateAuthorsCommand")]
//    [ProducesResponseType(StatusCodes.Status201Created)]
//    [ProducesResponseType(StatusCodes.Status400BadRequest)]
//    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
//    public async Task<ActionResult> Post(CreateAuthorsCommand command)
//    {
//        var response = await Mediator.Send(command);
//        return CreatedAtAction(nameof(Get), new { response.Key }, response);
//    }

//    /// <summary>
//    /// Update Authors Command
//    /// </summary>
//    /// <remarks>
//    /// Sample request:
//    ///
//    ///     HttpPut Body
//    ///     {
//    ///        "Key": "60fb5e99-3a78-43df-a512-7d8ff498499e",
//    ///        "Message":  "Hi, I am interested in learning about Semantic Kernel.",
//    ///        "Content": "Certainly! Semantic Kernel is a great framework for AI.",
//    ///     }
//    ///
//    ///     "version":  1.0
//    /// </remarks>
//    /// <param name="command"></param>
//    /// <returns>    
//    ///     {
//    ///        "Key": "60fb5e99-3a78-43df-a512-7d8ff498499e",
//    ///        "Message":  "Hi, I am interested in learning about Semantic Kernel.",
//    ///        "Content": "Certainly! Semantic Kernel is a great framework for AI.",
//    ///     }</returns>
//    [HttpPut(Name = "UpdateAuthorsCommand")]
//    [ProducesResponseType(StatusCodes.Status204NoContent)]
//    [ProducesResponseType(StatusCodes.Status404NotFound)]
//    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
//    public async Task<ActionResult> Put(UpdateAuthorsCommand command)
//    {
//        await Mediator.Send(command);

//        return NoContent();
//    }

//    /// <summary>
//    /// Patch Chat Session Command
//    /// </summary>
//    /// <remarks>
//    /// Sample request:
//    ///
//    ///     HttpPatch Body
//    ///     {
//    ///        "Key": "60fb5e99-3a78-43df-a512-7d8ff498499e",
//    ///        "Title":  "Semantic Kernel Chat Session"
//    ///     }
//    ///
//    ///     "version":  1.0
//    /// </remarks>
//    /// <param name="key"></param>
//    /// <param name="command"></param>
//    /// <returns>NoContent</returns>
//    [HttpPatch("{key}", Name = "PatchAuthorsCommand")]
//    [ProducesResponseType(StatusCodes.Status204NoContent)]
//    [ProducesResponseType(StatusCodes.Status404NotFound)]
//    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
//    public async Task<ActionResult> Patch(Guid key, PatchAuthorsCommand command)
//    {
//        command.Key = key;
//        await Mediator.Send(command);

//        return NoContent();
//    }

//    /// <summary>Remove Authors Command</summary>
//    /// <remarks>
//    /// Sample request:
//    ///
//    ///        "Key": 60fb5e99-3a78-43df-a512-7d8ff498499e
//    ///        "api-version":  1.0
//    /// 
//    /// </remarks>
//    /// <returns>NoContent</returns>
//    [HttpDelete("{key}", Name = "RemoveAuthorsCommand")]
//    [ProducesResponseType(StatusCodes.Status204NoContent)]
//    [ProducesResponseType(StatusCodes.Status404NotFound)]
//    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
//    [ProducesDefaultResponseType]
//    public async Task<IActionResult> Delete(Guid key)
//    {
//        await Mediator.Send(new DeleteAuthorsCommand() { Key = key });

//        return NoContent();
//    }
//}