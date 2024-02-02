using Microsoft.AspNetCore.Mvc;
using MIST155ajax.Models;
using System.Diagnostics;

namespace MIST155ajax.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Jsontest()
        {
            return View();
        }

        public IActionResult Travel()
        {
            return View();
        }

        public IActionResult First()
        {
            return View();
        }
        public IActionResult Register()
        {
            return View();
        }

        public IActionResult Address()
        {
            return View();
        }

        public IActionResult Avatar()
        {
            return View();
        }

        public IActionResult Spots()
        {
            return View();
        }

        public IActionResult AutoComplete()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public IActionResult Cors()
        {
            return View();
        }
    }
}
