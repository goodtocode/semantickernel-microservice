using Goodtocode.SemanticKernel.Presentation.Blazor.Client.Services;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("http://localhost") });
builder.Services.AddScoped<IChatService, ChatService>();

await builder.Build().RunAsync();
