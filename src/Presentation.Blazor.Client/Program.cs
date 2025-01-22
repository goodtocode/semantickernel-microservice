using Goodtocode.Presentation.WebApi.Client;
using Goodtocode.SemanticKernel.Presentation.Blazor.Client.Services;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddScoped(builder => new WebApiClient("http://localhost", new HttpClient()));
builder.Services.AddScoped<IChatService, ChatService>();

await builder.Build().RunAsync();
