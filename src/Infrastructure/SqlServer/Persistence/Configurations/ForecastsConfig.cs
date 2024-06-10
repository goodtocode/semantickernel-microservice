using Goodtocode.SemanticKernel.Core.Domain.Forecasts.Entities;

namespace Goodtocode.SemanticKernel.Infrastructure.SqlServer.Persistence.Configurations;

public class ForecastsConfig : IEntityTypeConfiguration<Forecast>
{
    public void Configure(EntityTypeBuilder<Forecast> builder)
    {
        builder.ToTable("Forecasts");
        builder.HasKey(x => x.Key);
        builder.Property(x => x.Key);
        builder.Property(x => x.TemperatureF);
        builder.Property(x => x.Summary).HasMaxLength(40);
        builder.Property(x => x.ZipCodesSearch).HasMaxLength(50);
        builder.Property(x => x.DateAdded);
        builder.Property(x => x.DateUpdated);
        builder
            .HasMany(x => x.ZipCodes)
            .WithOne(x => x.WeatherForecast);
    }
}