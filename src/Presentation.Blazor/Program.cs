using Azure.Monitor.OpenTelemetry.AspNetCore;
using Goodtocode.SemanticKernel.Presentation.Blazor.Components.Auth.Routing;
using Goodtocode.SemanticKernel.Presentation.Blazor;
using Goodtocode.SemanticKernel.Presentation.Blazor.Components;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.FluentUI.AspNetCore.Components;

var builder = WebApplication.CreateBuilder(args);

builder.AddLocalEnvironment();

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddFluentUIComponents();

builder.Services.AddUserClaimsSyncService();

builder.Services.AddAuthenticationForDownstream(builder.Configuration);

builder.Services.AddAuthorization();

builder.Services.AddOpenTelemetry().UseAzureMonitor(options =>
{
    options.ConnectionString = builder.Configuration["ApplicationInsights:ConnectionString"];
});

builder.Services.AddHttpContextAccessor();

builder.Services.AddBackendApi(builder.Configuration);

builder.Services.AddFrontendServices();

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

app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
});

app.UseAuthentication();
app.UseAuthorization();

app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.MapGroup("/authentication").MapSignInSignOut();

app.Run();
