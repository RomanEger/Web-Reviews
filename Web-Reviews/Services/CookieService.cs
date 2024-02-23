using Shared.DataTransferObjects;
using Web_Reviews.Services.Contracts;

namespace Web_Reviews.Services;

public class CookieService : ICookieService<TokenDTO, string>
{
    private IHttpClientFactory _clientFactory;
    
    public CookieService(IHttpClientFactory clientFactory)
    {
        _clientFactory = clientFactory;
    }
    
    public async Task AppendCookieAsync(TokenDTO tokenDto)
    {
        if (string.IsNullOrEmpty(tokenDto.AccessToken))
            throw new Exception("Null or empty AccessToken");
        var httpClient = _clientFactory.CreateClient();
        var response = await httpClient.PostAsJsonAsync("https://localhost:7224/cookie", tokenDto);
    }

    public async Task<string> Get(string key)
    {
        var httpClient = _clientFactory.CreateClient();
        var request = await httpClient.GetAsync("https://localhost:7224/token");
        var resultCookie = await request.Content.ReadAsStringAsync();
        return resultCookie;
    }
}