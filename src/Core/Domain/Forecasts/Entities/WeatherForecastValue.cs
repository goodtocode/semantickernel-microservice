namespace Goodtocode.SemanticKernel.Core.Domain.Forecasts.Entities;

public class ForecastValue : ValueObject
{


    private ForecastValue(Guid key, DateTime date, int temperatureF, List<int> zipCodes)
    {
        Key = key;
        Date = date;
        ZipCodes = zipCodes;
        TemperatureF = temperatureF;
        
    }

    public Guid Key { get; }

    public DateTime Date { get; }

    public int TemperatureF { get; }
    
    public List<int> ZipCodes { get; }
    
    public static Result<ForecastValue> Create(Guid key, DateTime date, int temperatureF, List<int> zipCodes)
    {
        if (key == Guid.Empty)
            return Result.Failure<ForecastValue>("key cannot be empty");

        if (date == DateTime.MinValue)
            Result.Failure<ForecastValue>("Date cannot be minimum value");

        return Result.Success(new ForecastValue(key, date, temperatureF, zipCodes));
    }

    protected override IEnumerable<IComparable> GetEqualityComponents()
    {
        yield return Key;
        yield return Date;
        yield return TemperatureF;
        yield return (IComparable)ZipCodes;
    }
}