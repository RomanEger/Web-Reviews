using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Shared.DataTransferObjects;
using Web_Reviews.Components;
using Web_Reviews.Services;
using Web_Reviews.Services.Contracts;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options => options.LoginPath = "/login");

builder.Services.AddAuthorization();

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddHttpClient();

builder.Services.AddScoped<ICookieService<TokenDTO, string>, CookieService>();

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
    var option = new CookieOptions()
    {
        Expires = DateTime.Now.AddMinutes(30)
    };
    context.Response.Cookies.Append(CookieKeys.AccessTokenKey, token.AccessToken, option);
});

app.MapGet("/token",  (HttpContext context) =>
{
    var cookie = context.Request.Cookies[CookieKeys.AccessTokenKey] ?? "";
    if (string.IsNullOrWhiteSpace(cookie))
        return Results.Unauthorized();
    
    return Results.Ok(cookie);
});


app.Run();
