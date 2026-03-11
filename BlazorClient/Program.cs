using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using BlazorClient;
using Microsoft.AspNetCore.Components;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AddScoped(sp =>
{
    var navigationManager = sp.GetRequiredService<NavigationManager>();
    var currentUrl = navigationManager.Uri;
    var baseUrl = GetBaseUrl(currentUrl);

    return new HttpClient { BaseAddress = new Uri(baseUrl) };
});

await builder.Build().RunAsync();

static string GetBaseUrl(string? currentUrl = null)
{
    if (string.IsNullOrEmpty(currentUrl))
    {
        return "http://localhost:7071/api/";
    }

    // If running on localhost, use the local functions API
    if (currentUrl.Contains("localhost", StringComparison.OrdinalIgnoreCase))
    {
        return "http://localhost:7071/api/";
    }

    // For Azure Static Web App or production, extract origin and construct absolute URL
    var uri = new Uri(currentUrl);
    return $"{uri.Scheme}://{uri.Host}{(uri.Port != 80 && uri.Port != 443 ? $":{uri.Port}" : "")}/api/";
}