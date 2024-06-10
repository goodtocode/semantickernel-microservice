using SemanticKernelMicroservice.Core.Application.Common.Exceptions;
using SemanticKernelMicroservice.Core.Application.ForecastLists.Queries.GetAll;
using SemanticKernelMicroservice.Core.Domain.Forecasts.Entities;
using SemanticKernelMicroservice.Infrastructure.SqlServer.Persistence;

namespace SemanticKernelMicroservice.Specs.Application.Unit.ForecastLists.Queries.GetAll;

[Binding]
[Scope(Tag = "getAllForecastsQuery")]
public class GetAllForecastsQueryStepDefinitions : TestBase
{
    private bool _foreacastExists;
    private bool _forecastWithZipcodeFilterExists;
    private ForecastsVm _response = new();
    private string _zipcodeFilter = string.Empty;


    [Given(@"I have a definition ""([^""]*)""")]
    public void GivenIHaveADefinition(string def)
    {
        _def = def;
    }

    [Given(@"Forecasts Exist ""([^""]*)""")]
    public void GivenForecastsExist(bool forecastExists)
    {
        _foreacastExists = forecastExists;
    }

    [Given(@"I have a zipcode filter ""([^""]*)""")]
    public void GivenIHaveAZipcodeFilter(string zipcodeFilter)
    {
        _zipcodeFilter = zipcodeFilter;
    }


    [Given(@"A forecast with zipcodeFilter exists ""([^""]*)""")]
    public void GivenAForecastWithZipcodeFilterExists(bool forecastWithZipcodeFilterExists)
    {
        _forecastWithZipcodeFilterExists = forecastWithZipcodeFilterExists;
    }

    [Given(@"I have a forecast exists ""([^""]*)""")]
    public void GivenIHaveAForecastExists(bool forecastExists)
    {
        _foreacastExists = true;
    }

    [When(@"I get all forecasts")]
    public async Task WhenIGetAWeatherForecast()
    {
        var context = new SemanticKernelMicroserviceContext(new DbContextOptionsBuilder<SemanticKernelMicroserviceContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options);

        if (_foreacastExists)
        {
            var zipCodes = _forecastWithZipcodeFilterExists ? _zipcodeFilter : "";

            if (_forecastWithZipcodeFilterExists)
            {
                var weatherForecastView = CreateForecastWithExistingZipcode();
                context.ForecastViews.Add(weatherForecastView);
            }

            for (var i = 0; i < 10; i++)
            {
                var weatherForecastView = CreateIncrementedWeatherForecast(i);
                context.ForecastViews.Add(weatherForecastView);
            }

            await context.SaveChangesAsync();
        }

        var request = new GetAllForecastsQuery() { ZipcodeFilter = _zipcodeFilter };

        var validator = new GetAllForecastsQueryValidator();

        _validationResponse = validator.Validate(request);

        if (_validationResponse.IsValid)
            try
            {
                var handler = new GetAllWeatherForecastQueryHandler(context, Mapper);
                _response = await handler.Handle(request, CancellationToken.None);
                _responseType = CommandResponseType.Successful;
                _response = await handler.Handle(request, CancellationToken.None);
            }
            catch (Exception e)
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
            }
        else
            _responseType = CommandResponseType.BadRequest;
    }

    private static ForecastsView CreateIncrementedWeatherForecast(int i)
    {
        var weatherForecastView = new ForecastsView
        {
            Key = Guid.NewGuid(),
            DateAdded = DateTime.Now,
            TemperatureF = 75,
            Summary = "summary",
            ZipCodesSearch = $"9260{i}"
        };
        return weatherForecastView;
    }

    private ForecastsView CreateForecastWithExistingZipcode()
    {
        var weatherForecastView = new ForecastsView
        {
            Key = Guid.NewGuid(),
            DateAdded = DateTime.Now,
            TemperatureF = 75,
            Summary = "summary",
            ZipCodesSearch = _zipcodeFilter
        };
        return weatherForecastView;
    }

    [Then(@"The response is ""([^""]*)""")]
    public void ThenTheResponseIs(string response)
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

    [Then(@"The response has a collection of forecasts")]
    public void ThenTheResponseHasACollectionOfSemanticKernelMicroservice()
    {
        if (_foreacastExists)
            _response.Forecasts.Any().Should().BeTrue();
        else
            _response.Forecasts.Any().Should().BeFalse();
    }

    [Then(@"Each forecast has a Key")]
    public void ThenEachForecastHasAKey()
    {
        foreach (var forecast in _response.Forecasts) forecast.Key.Should().NotBeEmpty();
    }

    [Then(@"Each forecast has a Date")]
    public void ThenEachForecastHasADate()
    {
        foreach (var forecast in _response.Forecasts) forecast.Date.Should();
    }

    [Then(@"Each forecast has a TemperatureC")]
    public void ThenEachForecastHasATemperatureC()
    {
        foreach (var forecast in _response.Forecasts) forecast.TemperatureC.Should();
    }

    [Then(@"Each forecast has a TemperatureF")]
    public void ThenEachForecastHasATemperatureF()
    {
        foreach (var forecast in _response.Forecasts) forecast.TemperatureF.Should();
    }

    [Then(@"Each forecast has a Summary")]
    public void ThenEachForecastHasASummary()
    {
        foreach (var forecast in _response.Forecasts) forecast.Summary.Should();
    }

    [Then(@"Each forecast has a collection of Zipcodes")]
    public void ThenEachWeatherForecastHasAZipcodes()
    {
        foreach (var forecast in _response.Forecasts) forecast.ZipCodes.Should();
    }

}