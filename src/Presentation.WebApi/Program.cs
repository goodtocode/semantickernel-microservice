using Azure.Extensions.AspNetCore.Configuration.Secrets;
using Azure.Identity;
using Azure.Monitor.OpenTelemetry.AspNetCore;
using Azure.Security.KeyVault.Secrets;
using Goodtocode.SemanticKernel.Core.Application;
using Goodtocode.SemanticKernel.Infrastructure.SemanticKernel;
using Goodtocode.SemanticKernel.Infrastructure.SqlServer;
using Goodtocode.SemanticKernel.Presentation.WebApi;

[assembly: ApiConventionType(typeof(DefaultApiConventions))]

var builder = WebApplication.CreateBuilder(args);

builder.AddLocalEnvironment();

// ToDo: Setup Authentication with Bearer Token
// Use for B2C
//builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
//    .AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAdB2C"));
// Use for Azure AD Client Credential Flow
//builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
//    .AddMicrosoftIdentityWebApi(builder.Configuration.GetSection(AppConfigurationKeys.AzureAdSectionKey));

builder.Services.AddApplicationServices();
builder.Services.AddDbContextServices(builder.Configuration);
builder.Services.AddSemanticKernelOpenAIServices(builder.Configuration);
builder.Services.AddWebUIServices();
builder.Services.AddHealthChecks();
BuildApiVerAndApiExplorer(builder);

builder.Services.AddOpenTelemetry().UseAzureMonitor(options =>
{
    options.ConnectionString = builder.Configuration["ApplicationInsights:ConnectionString"];
});

var app = builder.Build();

if (app.Environment.IsDevelopment() || app.Environment.IsLocal())
{
    app.UseSwagger();
    UseSwaggerUiConfigs();
    //using var scope = app.Services.CreateScope();
    //var initializer = scope.ServiceProvider.GetRequiredService<SemanticKernelMicroserviceDbContextInitializer>();
    //await initializer.InitialiseAsync();
    //await initializer.SeedAsync();
}

app.UseRouting();
app.UseStaticFiles();
app.UseHealthChecks("/health");
app.UseHttpsRedirection();
// ToDo: Setup Authentication with Bearer Token
//app.UseAuthorization();
//app.UseAuthentication();
app.MapControllers();
app.UseCors("AllowOrigin");
app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
app.Run();

void UseSwaggerUiConfigs()
{
    var providers = app.Services.GetService<IApiVersionDescriptionProvider>();

    app.UseSwaggerUI(options =>
    {
        if (providers == null) return;
        foreach (var description in providers.ApiVersionDescriptions)
            options.SwaggerEndpoint($"../swagger/{description.GroupName}/swagger.json",
                description.GroupName.ToUpperInvariant());
    });
}

void BuildApiVerAndApiExplorer(WebApplicationBuilder webApplicationBuilder)
{
    webApplicationBuilder.Services.AddApiVersioning(setup =>
    {
        setup.DefaultApiVersion = new ApiVersion(1, 0);
        setup.AssumeDefaultVersionWhenUnspecified = true;
        setup.ReportApiVersions = true;
    })
    .AddApiExplorer(setup =>
    {
        setup.GroupNameFormat = "'v'VVV";
        setup.SubstituteApiVersionInUrl = true;
    });
}