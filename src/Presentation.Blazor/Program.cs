using Goodtocode.SemanticKernel.Presentation.Blazor;
using Goodtocode.SemanticKernel.Presentation.Blazor.Components;
using Goodtocode.SemanticKernel.Presentation.Blazor.Pages.Chat.Services;
using Goodtocode.SemanticKernel.Presentation.Blazor.Services;

var builder = WebApplication.CreateBuilder(args);

builder.AddLocalEnvironment();

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddPresentationWebApiServices(builder.Configuration);

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ILocalStorageService, LocalStorageService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IChatService, ChatService>();

builder.Services.AddApplicationInsightsTelemetry(options =>
{
    options.ConnectionString = builder.Configuration["ApplicationInsights:ConnectionString"];
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts(); // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
}

app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
