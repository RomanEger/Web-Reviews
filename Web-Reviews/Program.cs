using Microsoft.AspNetCore.Authentication.Cookies;
using Shared.DataTransferObjects;
using Web_Reviews.Components;
using Web_Reviews.Services;
using Web_Reviews.Services.Contracts;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options => options.LoginPath = "/login");

builder.Services.AddAuthorization();

builder.Services.AddHttpClient();

builder.Services.AddScoped<ICookieService<TokenDTO, string>, CookieService>();

builder.Services.AddSingleton<Connections>();

var app = builder.Build();


if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/error");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.UseAuthentication();
app.UseAuthorization();

app.MapPost("/cookie", (TokenDTO token, HttpContext context) =>
{
    
    context.Response.Cookies.Append(CookieKeys.AccessTokenKey, token.AccessToken, new CookieOptions()
    {
        Expires = DateTimeOffset.Now.AddMinutes(30)
    });
});


app.MapGet("/token",  (HttpContext context) =>
{
    //var cookie = context.Request.Cookies[CookieKeys.AccessTokenKey] ?? "";
    context.Request.Cookies.TryGetValue(CookieKeys.AccessTokenKey, out string? cookie);
    if (string.IsNullOrWhiteSpace(cookie))
        return Results.Unauthorized();
    
    return Results.Ok(cookie);
});


app.Run();
