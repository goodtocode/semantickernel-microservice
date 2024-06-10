namespace Goodtocode.SemanticKernel.Presentation.WebApi.Common;

/// <summary>
/// Sets up ISender Mediator property
/// </summary>
[ApiController]
[ApiExceptionFilter]
[Route("api/[controller]")]
public abstract class ApiControllerBase : ControllerBase
{
    private ISender? _mediator;

    /// <summary>
    /// Mediator property exposing ISender type
    /// </summary>
    protected ISender Mediator => _mediator ??= HttpContext.RequestServices.GetRequiredService<ISender>();
}
