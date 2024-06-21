﻿using Goodtocode.SemanticKernel.Core.Application.Common.Exceptions;
using Goodtocode.SemanticKernel.Core.Application.Common.Mappings;
using Goodtocode.SemanticKernel.Infrastructure.SemanticKernel.Options;
using Goodtocode.SemanticKernel.Infrastructure.SqlServer.Persistence;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System.Reflection;

namespace Goodtocode.SemanticKernel.Specs.Integration;

public abstract class TestBase
{
    public enum CommandResponseType
    {
        Successful,
        BadRequest,
        NotFound,
        Error
    }

    internal string _def { get; set; } = string.Empty;
    internal IDictionary<string, string[]> _commandErrors = new ConcurrentDictionary<string, string[]>();
    internal CommandResponseType _responseType;
    internal ValidationResult _validationResponse = new();
    internal ChatCompletionContext _contextChatCompletion;
    internal IConfiguration _configuration;
    internal OpenAI _optionsOpenAi = new();

    public TestBase()
    {
        Mapper = new MapperConfiguration(cfg => { cfg.AddProfile<MappingProfile>(); })
            .CreateMapper();

        _contextChatCompletion = new ChatCompletionContext(new DbContextOptionsBuilder<ChatCompletionContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options);

        var executingType = Assembly.GetExecutingAssembly().GetTypes()
            .FirstOrDefault(x => x.Name == "AutoGeneratedProgram") ?? typeof(TestBase);
        _configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.test.json", optional: true, reloadOnChange: true)
            .AddUserSecrets(executingType.Assembly, optional: true)
            .AddEnvironmentVariables()
            .Build();

        _configuration.GetSection(nameof(OpenAI)).Bind(_optionsOpenAi);
    }

    internal IMapper Mapper { get; }

    internal CommandResponseType HandleAssignResponseType
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

    internal void HandleHasResponseType(string response)
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

    internal void HandleExpectedValidationErrorsAssertions(string expectedErrors)
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