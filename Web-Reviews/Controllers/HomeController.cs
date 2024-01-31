using Microsoft.AspNetCore.Mvc;

namespace Web_Reviews.Controllers;

public class HomeController
{
    [HttpGet]
    [Route("")] [Route("[action]")]
    public string Index()
    {
        return "Hello";
    }
}