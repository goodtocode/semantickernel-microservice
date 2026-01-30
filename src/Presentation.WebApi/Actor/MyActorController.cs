using Goodtocode.SemanticKernel.Core.Application.Actor;
using Goodtocode.SemanticKernel.Presentation.WebApi.Common;
using Microsoft.AspNetCore.Authorization;

namespace Goodtocode.SemanticKernel.Presentation.WebApi.Actor;

/// <summary>
/// Actor endpoints to create a chat, continue a chat, delete a chat and retrieve chat history
/// </summary>
[ApiController]
[ApiConventionType(typeof(DefaultApiConventions))]
[Route("api/v{version:apiVersion}my/actors")]
[ApiVersion("1.0")]
[Authorize]
public class MyActorController : ApiControllerBase
{
    /// <summary>
    /// Retrieves the actor profile by external ID, including session history.
    /// </summary>
    /// <remarks>
    /// Sample request:
    ///
    ///        "Id": 60fb5e99-3a78-43df-a512-7d8ff498499e
    ///        "api-version":  1.0
    /// 
    /// </remarks>
    /// <returns>
    /// ActorDto
    ///     { 
    ///         Id: 1efb5e99-3a78-43df-a512-7d8ff498499e
    ///         Name: John Doe
    ///     }
    /// </returns>
    [HttpGet(Name = "GetMyActorProfile")]
    [ProducesResponseType(typeof(ActorDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActorDto> GetMyActorProfile()
    {
        return await Mediator.Send(new GetMyActorQuery());
    }

    /// <summary>
    /// Creates a new actor session with empty history.
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
    /// <param name="command">The command containing actor creation details.</param>
    /// <returns>
    /// The created ActorDto object.
    /// </returns>
    [HttpPost(Name = "SaveMyActor")]
    [ProducesResponseType<ActorDto>(StatusCodes.Status200OK)]
    [ProducesResponseType<ActorDto>(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> SaveMyActor(SaveMyActorCommand command)
    {
        var response = await Mediator.Send(command);
        return CreatedAtAction(nameof(GetMyActorProfile), new { response.OwnerId }, response);
    }
}