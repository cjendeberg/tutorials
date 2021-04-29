using Microsoft.AspNetCore.Mvc;

namespace Zero99Lotto.SRC.Services.Draws.API.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return new RedirectResult("~/swagger");
        }
    }
}