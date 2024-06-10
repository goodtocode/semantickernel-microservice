using MediatR;
using $safeprojectname$;
using Microservice.Persistence;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration.AzureAppConfiguration;
using Microsoft.Identity.Web;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);
var environment = Environment.GetEnvironmentVariable(EnvironmentVariableKeys.EnvironmentAspNetCore) ?? EnvironmentVariableDefaults.Environment;
builder.Configuration
    .AddJsonFile("appsettings.json")
    .AddJsonFile($"appsettings.{environment}.json");

var connection = Environment.GetEnvironmentVariable(EnvironmentVariableKeys.AppConfigurationConnection);
if (connection?.Length > 0)
{
    builder.Configuration.AddAzureAppConfiguration(options =>
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

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApi(builder.Configuration.GetSection(AppConfigurationKeys.AzureAdSectionKey));
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddApiVersioning(setupAction =>
{
    setupAction.AssumeDefaultVersionWhenUnspecified = true;
    setupAction.DefaultApiVersion = new ApiVersion(1, 0);
    setupAction.ReportApiVersions = true;
});
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Microservice",
        Version = "v1",
        Description = "RESTful Microservice APIs",
        Contact = new OpenApiContact
        {
            Name = "developers@goodtocode.com",
            Email = "developers@goodtocode.com",
            Url = new Uri("https://github.com/goodtocode"),
        },
    });
});
builder.Services.AddMediatR(typeof(Microservice.Application.BusinessCreateCommand).Assembly);
builder.Services.AddPersistence(builder.Configuration, AppConfigurationKeys.SqlConnectionKey);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
}

using (var scope = app.Services.CreateScope())
{
    var dataContext = scope.ServiceProvider.GetRequiredService<AssociateDbContext>();
    dataContext.Database.Migrate();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();

