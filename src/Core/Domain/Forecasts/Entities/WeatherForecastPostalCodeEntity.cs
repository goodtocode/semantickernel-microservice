using Goodtocode.Domain.Types;

namespace Goodtocode.SemanticKernel.Core.Domain.Forecasts.Entities;

public class WeatherForecastPostalCodeEntity : DomainEntity<WeatherForecastPostalCodeEntity>
{
    protected WeatherForecastPostalCodeEntity() { }

    public WeatherForecastPostalCodeEntity(int zipCode, Forecast weatherForecast)
    {
        ZipCode = zipCode;
        WeatherForecast = weatherForecast;
    }

    public virtual int ZipCode { get; private set; }

    public virtual Forecast WeatherForecast { get; private set; }
}