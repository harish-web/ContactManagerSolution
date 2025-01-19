using Microsoft.AspNetCore.Mvc;

namespace ContactManager.UI.Areas.Admin.Controllers
{
    [Area("Admin")]
    //[Route("[Controller]")]
    public class HomeController : Controller
    {
        //[Route("[action]")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
