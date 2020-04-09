using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using HexaGone.Models;

namespace HexaGone.Controllers
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
            return View(new UserModel());
        }

        [HttpPost]
        public IActionResult Index(HexaGone.Models.UserModel user)
        {
           
            return Content("blubb");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Game()
        {
            Models.Map map = new Map(Map.mapModeBiomes, 50, 50, Map.Medium);

            //===
            // Ausfüllen:
            map.HexSideLength = 30;
            //===


            return View("Game", map);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
