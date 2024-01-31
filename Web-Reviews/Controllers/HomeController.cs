using Microsoft.AspNetCore.Mvc;

namespace Web_Reviews.Controllers;

public class HomeController : Controller
{
    [HttpGet]
    [Route("")] [Route("[action]")]
    public IActionResult Index()
    {
        return View();
    }
}