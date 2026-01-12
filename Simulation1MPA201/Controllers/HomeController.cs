using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Simulation1MPA201.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }


    }
}
