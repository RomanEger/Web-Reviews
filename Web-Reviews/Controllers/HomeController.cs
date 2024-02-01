using Microsoft.AspNetCore.Mvc;

namespace Web_Reviews.Controllers;

public class HomeController : Controller
{
    // private IRepository _repository;
    //
    // public HomeController(IRepository repository)
    // {
    //     _repository = repository;
    // }


    [HttpGet]
    [Route("")]
    [Route("[action]")]
    public IActionResult Index() => View();

    // [HttpGet]
    // public IActionResult Auth() => View();

    // [HttpGet]
    // public IActionResult Login(Guid id) => View(model:repository.GetUser(id));

    // [HttpPost]
    // public IActionResult Auth(string login, string password)
    // {
    //     var user = repository.GetUser(login);
    //     if(user?.Password == password)
    //         return View("Index",model:user.Id);
    //     else
    //     {
    //         ViewBag.DataContentBag = "Некорректный логин или пароль. Попробуйте снова";
    //         return View();
    //     }
    // }
}