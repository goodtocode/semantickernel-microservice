using Goodtocode.SemanticKernel.Presentation.Blazor.Client.Services;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Presentation.Blazor.Rcl;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddPresentationWebApiServices(builder.Configuration);
builder.Services.AddScoped<IChatService, ChatService>();

await builder.Build().RunAsync();
