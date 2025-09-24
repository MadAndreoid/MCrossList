using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Radzen;
using MCrossList.Client;
using Microsoft.AspNetCore.Components.Authorization;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.Services.AddRadzenComponents();
builder.Services.AddRadzenCookieThemeService(options =>
{
    options.Name = "MCrossListTheme";
    options.Duration = TimeSpan.FromDays(365);
});
builder.Services.AddScoped<MCrossList.Client.dbService>();
builder.Services.AddAuthorizationCore();
builder.Services.AddHttpClient("MCrossList.Server", client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress));
builder.Services.AddTransient(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("MCrossList.Server"));
builder.Services.AddScoped<MCrossList.Client.SecurityService>();
builder.Services.AddScoped<AuthenticationStateProvider, MCrossList.Client.ApplicationAuthenticationStateProvider>();
builder.Services.AddScoped<MCrossList.Client.Services.IVintedService, MCrossList.Client.Services.VintedService>();
var host = builder.Build();
await host.RunAsync();