using System.Net;
using System.Security.Claims;
using System.Text.Json;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Shared.DataTransferObjects;

namespace Web_Reviews.Controllers;

[Route("{action}")]
public class AuthController(HttpClient httpClient, IConfiguration configuration) : Controller
{
    private readonly string _apiConnection = configuration["Api"] ?? string.Empty;

    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(UserForAuthenticationDTO user)
    {
        var response = await httpClient.PostAsJsonAsync($"{_apiConnection}/api/authentication/login", user);

        if (response.StatusCode != HttpStatusCode.OK)
            return StatusCode(int.Parse(response.StatusCode.ToString()));
        
        var result = await response.Content.ReadAsStringAsync();

        var tokenDto = JsonSerializer.Deserialize<TokenDTO>(result, new JsonSerializerOptions()
        {
            PropertyNameCaseInsensitive = true
        }) ?? null;
        if (tokenDto is null)
            return Unauthorized();
        List<Claim>  claims= 
        [ 
            new(ClaimTypes.Anonymous, tokenDto.AccessToken), 
            new(ClaimTypes.UserData, tokenDto.RefreshToken) 
        ];
            
        var claimsIdentity = new ClaimsIdentity(claims, "Cookies");

        var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
            
        await HttpContext.SignInAsync(claimsPrincipal);
        
        return RedirectPermanent($"/");
    }

    [HttpGet]
    public IActionResult Profile()
    {
        if (HttpContext.User.Identity is not null && HttpContext.User.Identity.IsAuthenticated)
            return View();
        return RedirectToAction("Login");
    }
}