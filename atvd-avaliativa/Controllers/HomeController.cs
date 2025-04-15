using System.Diagnostics;
using atvd_avaliativa.Models;
using Microsoft.AspNetCore.Mvc;

namespace atvd_avaliativa.Controllers
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

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public IActionResult AcessoNegado()
        {
            ViewBag.Message = "Você não tem acesso a esta página.";
            return View();
        }

    }
}
