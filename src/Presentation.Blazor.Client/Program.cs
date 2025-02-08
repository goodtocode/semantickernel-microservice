using Goodtocode.SemanticKernel.Presentation.Blazor.Client.Services;
using Goodtocode.SemanticKernel.Presentation.Blazor.Rcl;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddPresentationWebApiServices(builder.Configuration);
builder.Services.AddScoped<IChatService, ChatService>();

await builder.Build().RunAsync();
