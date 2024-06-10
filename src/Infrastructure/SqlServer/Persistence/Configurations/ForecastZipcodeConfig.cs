using Goodtocode.SemanticKernel.Core.Domain.Forecasts.Entities;

namespace Goodtocode.SemanticKernel.Infrastructure.SqlServer.Persistence.Configurations;

public class ForecastZipcodeConfig : IEntityTypeConfiguration<WeatherForecastPostalCodeEntity>
{
    public void Configure(EntityTypeBuilder<WeatherForecastPostalCodeEntity> builder)
    {
        builder.ToTable("ForecastZipCodes");
        builder.HasKey(x => x.Key);
        builder.Property(x => x.Key);
        builder
            .HasOne(x => x.WeatherForecast)
            .WithMany(x => x.ZipCodes);
    }
}