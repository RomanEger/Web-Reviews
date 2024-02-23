namespace Web_Reviews.Services.Contracts;

public interface ICookieService<T,K> 
{
    Task AppendCookieAsync(T obj);

    Task<K> Get(string key);
}