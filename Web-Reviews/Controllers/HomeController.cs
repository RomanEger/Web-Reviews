using Microsoft.AspNetCore.Mvc;

namespace Web_Reviews.Controllers;

[Route("")]
public class HomeController : Controller
{
    [Route("")]
    [Route("Home")]
    [Route("Index")]
    public IActionResult Index()
    {
        return View();
    }
}