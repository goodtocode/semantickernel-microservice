using Azure.Monitor.OpenTelemetry.AspNetCore;
using Goodtocode.SemanticKernel.Presentation.Blazor;
using Goodtocode.SemanticKernel.Presentation.Blazor.Components;

var builder = WebApplication.CreateBuilder(args);

builder.AddLocalEnvironment();

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddBackEndApi(builder.Configuration);

builder.Services.AddBlazorServices();

builder.Services.AddHttpContextAccessor();

builder.Services.AddOpenTelemetry().UseAzureMonitor(options =>
{
    options.ConnectionString = builder.Configuration["ApplicationInsights:ConnectionString"];
});

var app = builder.Build();

if (app.Environment.IsDevelopment() || app.Environment.IsLocal())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts(); // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    // ToDo: Add CSP Header
}

app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
