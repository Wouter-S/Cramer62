using Microsoft.AspNetCore.Mvc;

namespace CramerGui.Controllers
{
    public class AdminController : Controller
    {
        [Route("Admin")]
        public IActionResult Index()
        {
            return File("~/admin.html", "text/html");
        }
    }
}
