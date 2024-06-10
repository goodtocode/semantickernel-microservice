using Goodtocode.Domain.Types;

namespace Goodtocode.SemanticKernel.Core.Domain.Forecasts.Entities;

public class Forecast : DomainEntity<Forecast>
{
    
    protected Forecast() { }

    public Forecast(ForecastValue weatherForecastAddValue) : base(weatherForecastAddValue.Key)
    {
        ForecastDate = weatherForecastAddValue.Date.ToUniversalTime();
        TemperatureF = weatherForecastAddValue.TemperatureF;
        Summary = SummaryFValue().ToString();
        ZipCodes = new List<WeatherForecastPostalCodeEntity>();
        foreach (var zipcode in weatherForecastAddValue.ZipCodes)
            ZipCodes.Add(new WeatherForecastPostalCodeEntity(zipcode, this));
        PopulateZipcodeSearch();
        DateAdded = DateTime.UtcNow;
    }

    public int TemperatureF { get; private set; }

    public int TemperatureC => 32 + (int)(TemperatureF / 0.5556);

    public string Summary { get; private set; }

    public string ZipCodesSearch { get; private set; }

    public virtual List<WeatherForecastPostalCodeEntity> ZipCodes { get; } = [];

    public DateTime ForecastDate { get; private set; }

    public DateTime DateAdded { get; private set; }

    public DateTime? DateUpdated { get; private set; }

    public void AddZipcode(int zipcode)
    {
        if (ZipCodes.Any(x => x.ZipCode == zipcode)) return;
        ZipCodes.Add(new WeatherForecastPostalCodeEntity(zipcode, this));
        PopulateZipcodeSearch();
        DateUpdated = DateTime.Now;
    }

    public void RemoveZipcode(int zipcode)
    {
        var existingZip = ZipCodes.FirstOrDefault(x => x.ZipCode == zipcode);
        if (existingZip == null) return;
        ZipCodes.Remove(existingZip);
        PopulateZipcodeSearch();
        DateUpdated = DateTime.Now;
    }

    public Result UpdateDate(DateTime dateTime)
    {
        if (dateTime == DateTime.MinValue)
            return Result.Failure("Date cannot be minimum value");
        ForecastDate = dateTime;
        DateUpdated = DateTime.UtcNow;
        return Result.Success();
    }

    public void UpdateTemperatureF(int temperatureF)
    {
        TemperatureF = temperatureF;
        DateUpdated = DateTime.UtcNow;
    }

    private SummaryType SummaryFValue()
    {
        return TemperatureF switch
        {
            <= 32 => SummaryType.Freezing,
            > 32 and <= 40 => SummaryType.Bracing,
            > 40 and <= 50 => SummaryType.Chilly,
            > 50 and <= 60 => SummaryType.Cool,
            > 60 and <= 70 => SummaryType.Mild,
            > 70 and <= 80 => SummaryType.Warm,
            > 80 and <= 90 => SummaryType.Balmy,
            > 90 and <= 100 => SummaryType.Hot,
            > 100 and <= 110 => SummaryType.Sweltering,
            > 110 => SummaryType.Scorching
        };
    }

    private void PopulateZipcodeSearch()
    {
        ZipCodesSearch = string.Join(", ", ZipCodes.Select(x => x.ZipCode));
    }

    public void UpdateZipcodes(List<int> requestZipcodes)
    {
        ZipCodes.Clear();
        foreach (var zipcode in requestZipcodes)
            ZipCodes.Add(new WeatherForecastPostalCodeEntity(zipcode, this));
        DateUpdated = DateTime.Now;
        PopulateZipcodeSearch();
    }

    public enum SummaryType
    {
        Freezing,
        Bracing,
        Chilly,
        Cool,
        Mild,
        Warm,
        Balmy,
        Hot,
        Sweltering,
        Scorching
    }
}