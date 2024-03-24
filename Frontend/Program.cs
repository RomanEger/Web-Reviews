using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Frontend.Components;
using Frontend.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.Configuration["FrontendUrl"] ?? "https://localhost:6000") });

builder.Services.AddSingleton<Connections, Connections>();

builder.Services.AddSingleton<ICookieService, CookieService>();

builder.Services.AddHttpClient(
    "Auth",
    options => options.BaseAddress = new Uri(builder.Configuration["BackendUrl"] ?? "https://localhost:5001"));

await builder.Build().RunAsync();
