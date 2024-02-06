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
    private HttpClient _client = new HttpClient();

    private readonly string _urlApi = "https://localhost:5084";
    [HttpGet]
    [Route("")]
    [Route("[action]")]
    public async Task<IActionResult> Index()
    {
        //var response = await _client.GetAsync(_urlApi + "/api/");
        //мб какая-то обработка, преобразование response
        // var obj = await response.Content.ReadAsStringAsync();
        // var data = JsonConvert.DeserializeObject<List<model>>(obj) ?? new();
        return View();
    }

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