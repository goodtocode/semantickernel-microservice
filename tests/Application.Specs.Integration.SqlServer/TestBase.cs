using Goodtocode.SemanticKernel.Core.Application.Common.Exceptions;
using Goodtocode.SemanticKernel.Core.Application.Common.Mappings;

namespace SemanticKernelMicroservice.Specs.Application.Unit;

public class TestBase
{
    public enum CommandResponseType
    {
        Successful,
        BadRequest,
        NotFound,
        Error
    }

    public string _def { get; set; } = "";
    public IDictionary<string, string[]> _commandErrors = new ConcurrentDictionary<string, string[]>();
    public CommandResponseType _responseType;
    public ValidationResult _validationResponse = new ValidationResult();

    public TestBase()
    {
        Mapper = new MapperConfiguration(cfg => { cfg.AddProfile<MappingProfile>(); })
            .CreateMapper();

        //SemanticKernelMicroserviceContext = new SemanticKernelMicroserviceContext(new DbContextOptionsBuilder<SemanticKernelMicroserviceContext>()
        //    .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options);
    }

    //public ISemanticKernelMicroserviceContext SemanticKernelMicroserviceContext { get; set; }

    public IMapper Mapper { get; }

    public CommandResponseType HandleAssignResponseType
        (Exception e)
    {
        switch (e)
        {
            case CustomValidationException validationException:
                _commandErrors = validationException.Errors;
                _responseType = CommandResponseType.BadRequest;
                break;
            case CustomNotFoundException notFoundException:
                _responseType = CommandResponseType.NotFound;
                break;
            default:
                _responseType = CommandResponseType.Error;
                break;
        }

        return _responseType;
    }

    public void HandleHasResponseType(string response)
    {
        switch (response)
        {
            case "Success":
                _responseType.Should().Be(CommandResponseType.Successful);
                break;
            case "BadRequest":
                _responseType.Should().Be(CommandResponseType.BadRequest);
                break;
            case "NotFound":
                _responseType.Should().Be(CommandResponseType.NotFound);
                break;
        }
    }

    public void HandleExpectedValidationErrorsAssertions(string expectedErrors)
    {
        var def = _def;

        if (string.IsNullOrWhiteSpace(expectedErrors)) return;

        var expectedErrorsCollection = expectedErrors.Split(",");

        foreach (var field in expectedErrorsCollection)
        {
            var hasCommandValidatorErrors = _validationResponse.Errors.Any(x => x.PropertyName == field.Trim());
            var hasCommandErrors = _commandErrors.Any(x => x.Key == field.Trim());
            var hasErrorMatch = hasCommandErrors || hasCommandValidatorErrors;
            hasErrorMatch.Should().BeTrue();
        }
    }

}