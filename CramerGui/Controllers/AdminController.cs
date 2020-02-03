using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;
using CramerAlexa.Repositories;
using CramerAlexa.Services;
using Microsoft.AspNetCore.Mvc;
using CramerAlexa.Controllers;

namespace CramerAlexa.Controllers
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
