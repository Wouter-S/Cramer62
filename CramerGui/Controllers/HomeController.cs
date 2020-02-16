using Microsoft.AspNetCore.Mvc;

namespace CramerGui.Controllers
{
    public class HomeController : Controller
    {
        [Route("Touch")]
        [Route("")]
        public IActionResult Touch()
        {
            return File("~/touch.html", "text/html");
        }

        [Route("Mobile")]
        public IActionResult Mobile()
        {
            return File("~/mobile.html", "text/html");

        }
    }
}
