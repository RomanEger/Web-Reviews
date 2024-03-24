using System.Text;
using Microsoft.JSInterop;

namespace Frontend.Services;

public class CookieService(IJSRuntime jsRuntime) : ICookieService
{
    public async Task AddCookie(string key, string value, TimeSpan? expireMS = null)
    {
        var sb = new StringBuilder($"document.cookie = \'{key}={value}\'");
        if (expireMS != null)
        {
            var exp = DateToUTC(expireMS.Value);
            sb.Append($";expires={exp}");
        }
        await jsRuntime.InvokeVoidAsync("eval", sb.ToString());
    }
    
    public async Task<IEnumerable<string>> GetCookie() => await jsRuntime.InvokeAsync<IEnumerable<string>>("eval", "document.cookie.split(\";\")");
    
    private static string DateToUTC(TimeSpan span) => DateTime.Now.Add(span).ToUniversalTime().ToString("R");
}