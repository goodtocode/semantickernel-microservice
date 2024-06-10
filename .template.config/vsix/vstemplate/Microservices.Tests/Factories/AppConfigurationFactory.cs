using Microservice.Api;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.AzureAppConfiguration;
using System;

namespace $safeprojectname$
{
    public class AppConfigurationFactory
    {
        public IConfiguration Configuration { get; private set; }
        public IConfiguration Create()
        {            
            var builder = new ConfigurationBuilder();
            var environment = Environment.GetEnvironmentVariable(EnvironmentVariableKeys.EnvironmentAspNetCore) ?? EnvironmentVariableDefaults.Environment;
            builder
                .AddJsonFile("appsettings.json")
                .AddJsonFile($"appsettings.{environment}.json");

            var connection = Environment.GetEnvironmentVariable(EnvironmentVariableKeys.AppConfigurationConnection);
            if (connection?.Length > 0)
            {
                builder.AddAzureAppConfiguration(options =>
                    options
                        .Connect(connection)
                        .ConfigureRefresh(refresh =>
                        {
                            refresh.Register(AppConfigurationKeys.SentinelKey, refreshAll: true)
                                    .SetCacheExpiration(new TimeSpan(0, 60, 0));
                        })
                        .Select(KeyFilter.Any, LabelFilter.Null)
                        .Select(KeyFilter.Any, environment));
            }

            Configuration = builder.Build();
            return Configuration;
        }
    }
}
