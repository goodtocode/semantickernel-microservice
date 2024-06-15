//namespace SemanticKernelMicroservice.Specs.Application.Unit.ForecastLists.Queries.GetPaginated;

//[Binding]
//[Scope(Tag = "getPaginatedSemanticKernelMicroserviceQuery")]
//public class GetPaginatedSemanticKernelMicroserviceQueryStepDefinitions : TestBase
//{
//    private bool _foreacastExists;
//    private PaginatedList<ForecastPaginatedDto> _response;
//    private string _zipcodeFilter = string.Empty;
//    private int _pageNumber;
//    private int _pageSize;

//    [Given(@"I have a definition ""([^""]*)""")]
//    public void GivenIHaveADefinition(string def)
//    {
//        _def = def;
//    }

//    [Given(@"I have a page number ""([^""]*)""")]
//    public void GivenIHaveAPageNumber(int pageNumber)
//    {
//        _pageNumber = pageNumber;
//    }

//    [Given(@"I have a forecast exists ""([^""]*)""")]
//    public void GivenIHaveAForecastExists(bool forecastExists)
//    {
//        _foreacastExists = true;
//    }

//    [Given(@"I have a page size ""([^""]*)""")]
//    public void GivenIHaveAPageSize(int pageSize)
//    {
//        _pageSize = pageSize;
//    }

//    [Given(@"A collection of Weather Forecasts Exist ""([^""]*)""")]
//    public void GivenACollectionOfSemanticKernelMicroserviceExist(bool forecastExists)
//    {
//        _foreacastExists = forecastExists;
//    }

//    [When(@"I get paginated weather forecasts")]
//    public async Task WhenIGetPaginatedSemanticKernelMicroservice()
//    {
//        var context = new SemanticKernelMicroserviceContext(new DbContextOptionsBuilder<SemanticKernelMicroserviceContext>()
//            .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options);

//        if (_foreacastExists)
//        {
//            for (var i = 0; i < 100; i++)
//            {
//                var weatherForecastView = CreateIncrementedWeatherForecast(i);
//                context.ForecastViews.Add(weatherForecastView);
//            }

//            await context.SaveChangesAsync();
//        }

//        var request = new GetForecastsPaginatedQuery()
//        {
//            PageNumber = _pageNumber,
//            PageSize = _pageSize
//        };

//        var validator = new GetForecastsPaginatedQueryValidator();

//        _validationResponse = validator.Validate(request);

//        if (_validationResponse.IsValid)
//            try
//            {
//                var handler = new GetSemanticKernelMicroservicePaginatedQueryHandler(context, Mapper);
//                _response = await handler.Handle(request, CancellationToken.None);
//                _responseType = CommandResponseType.Successful;
//                _response = await handler.Handle(request, CancellationToken.None);
//            }
//            catch (Exception e)
//            {
//                switch (e)
//                {
//                    case CustomValidationException validationException:
//                        _commandErrors = validationException.Errors;
//                        _responseType = CommandResponseType.BadRequest;
//                        break;
//                    case CustomNotFoundException notFoundException:
//                        _responseType = CommandResponseType.NotFound;
//                        break;
//                    default:
//                        _responseType = CommandResponseType.Error;
//                        break;
//                }
//            }
//        else
//            _responseType = CommandResponseType.BadRequest;
//    }

//    private static ForecastsView CreateIncrementedWeatherForecast(int i)
//    {
//        var weatherForecastView = new ForecastsView
//        {
//            Key = Guid.NewGuid(),
//            DateAdded = DateTime.Now,
//            TemperatureF = 75,
//            Summary = "summary",
//            ZipCodesSearch = $"9260{i}"
//        };
//        return weatherForecastView;
//    }

//    private ForecastsView CreateForecastWithExistingZipcode()
//    {
//        var weatherForecastView = new ForecastsView
//        {
//            Key = Guid.NewGuid(),
//            DateAdded = DateTime.Now,
//            TemperatureF = 75,
//            Summary = "summary",
//            ZipCodesSearch = _zipcodeFilter
//        };
//        return weatherForecastView;
//    }

//    [Then(@"The response is ""([^""]*)""")]
//    public void ThenTheResponseIs(string response)
//    {
//        switch (response)
//        {
//            case "Success":
//                _responseType.Should().Be(CommandResponseType.Successful);
//                break;
//            case "BadRequest":
//                _responseType.Should().Be(CommandResponseType.BadRequest);
//                break;
//            case "NotFound":
//                _responseType.Should().Be(CommandResponseType.NotFound);
//                break;
//        }
//    }

//    [Then(@"The response has a Page Number")]
//    public void ThenTheResponseHasAPageNumber()
//    {
//        if (_responseType != CommandResponseType.Successful) return;
//        _response.PageNumber.Should();
//    }

//    [Then(@"The response has a Total Pages")]
//    public void ThenTheResponseHasATotalPages()
//    {
//        if (_responseType != CommandResponseType.Successful) return;
//        _response.TotalPages.Should();
//    }

//    [Then(@"The response has a Total Count")]
//    public void ThenTheResponseHasATotalCount()
//    {
//        if (_responseType != CommandResponseType.Successful) return;
//        _response.TotalCount.Should();
//    }

//    [Then(@"The response has a collection of items")]
//    public void ThenTheResponseHasACollectionOfItems()
//    {
//        if (_responseType != CommandResponseType.Successful) return;
//        _response.Items.Should();
//    }

//    [Then(@"The response has a collection of weather forecasts")]
//    public void ThenTheResponseHasACollectionOfSemanticKernelMicroservice()
//    {
//        if (_responseType != CommandResponseType.Successful) return;

//        if (_foreacastExists)
//            _response.Items.Any().Should().BeTrue();
//        else
//            _response.Items.Any().Should().BeFalse();
//    }

//    [Then(@"Each weather forecast has a Key")]
//    public void ThenEachForecastHasAKey()
//    {
//        if (_responseType != CommandResponseType.Successful) return;
//        foreach (var forecast in _response.Items) forecast.Key.Should().NotBeEmpty();
//    }

//    [Then(@"Each weather forecast has a Date")]
//    public void ThenEachForecastHasADate()
//    {
//        if (_responseType != CommandResponseType.Successful) return;
//        foreach (var forecast in _response.Items) forecast.Date.Should();
//    }

//    [Then(@"Each weather forecast has a TemperatureC")]
//    public void ThenEachForecastHasATemperatureC()
//    {
//        if (_responseType != CommandResponseType.Successful) return;
//        foreach (var forecast in _response.Items) forecast.TemperatureC.Should();
//    }

//    [Then(@"Each weather forecast has a TemperatureF")]
//    public void ThenEachForecastHasATemperatureF()
//    {
//        if (_responseType != CommandResponseType.Successful) return;
//        foreach (var forecast in _response.Items) forecast.TemperatureF.Should();
//    }

//    [Then(@"Each weather forecast has a Summary")]
//    public void ThenEachForecastHasASummary()
//    {
//        if (_responseType != CommandResponseType.Successful) return;
//        foreach (var forecast in _response.Items) forecast.Summary.Should();
//    }

//    [Then(@"Each weather forecast has a Zipcodes")]
//    public void ThenEachWeatherForecastHasAZipcodes()
//    {
//        if (_responseType != CommandResponseType.Successful) return;
//        foreach (var forecast in _response.Items) forecast.ZipCodes.Should();
//    }


//    [Then(@"If the response has validation issues I see the ""([^""]*)"" in the response")]
//    public void ThenIfTheResponseHasValidationIssuesISeeTheInTheResponse(string expectedErrors)
//    {
//        HandleExpectedValidationErrorsAssertions(expectedErrors);
//    }

//}