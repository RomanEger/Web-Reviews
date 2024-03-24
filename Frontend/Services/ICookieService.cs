namespace Frontend.Services;

public interface ICookieService
{
    Task AddCookie(string key, string value, TimeSpan? expireMS = null);

    Task<IEnumerable<string>> GetCookie();
}