using Goodtocode.SemanticKernel.Presentation.Blazor;
using Goodtocode.SemanticKernel.Presentation.Blazor.Services;
using Goodtocode.SemanticKernel.Presentation.Blazor.Components;
using Goodtocode.SemanticKernel.Presentation.Blazor.Pages.Chat.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
    //.AddInteractiveWebAssemblyComponents();

builder.Services.AddPresentationWebApiServices(builder.Configuration);

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ILocalStorageService, LocalStorageService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IChatService, ChatService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    //app.UseWebAssemblyDebugging();
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
///AddInteractiveWebAssemblyRendermode()

app.Run();
