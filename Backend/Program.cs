using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Shared.DataTransferObjects;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(
    options => options.AddPolicy(
        "wasm",
        policy => policy.WithOrigins([builder.Configuration["BackendUrl"] ?? "https://localhost:5001", 
            builder.Configuration["FrontendUrl"] ?? "https://localhost:7107"])
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials()));

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddOpenApiDocument();

var app = builder.Build();

if (builder.Environment.IsDevelopment())
{
    app.UseOpenApi();
    app.UseSwaggerUi();
}

app.UseCors("wasm");

app.UseHttpsRedirection();

app.MapPost("/cookie", (TokenDTO token, HttpContext context) =>
{
    if(context.Request.Cookies.TryGetValue("AccessToken", out string? aToken))
        context.Response.Cookies.Delete("AccessToken");
    
    context.Response.Cookies.Append("AccessToken", token.AccessToken);
});

app.MapGet("/cookie", (HttpContext context) => context.Request.Cookies["AccessToken"] ?? string.Empty);

app.Run();