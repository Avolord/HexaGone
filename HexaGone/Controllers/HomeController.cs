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
            UnitStats.PopulateUnits();
            for (int i = 0; i < 100; i++)
            {
                Models.Army Attacker = new Models.Army();
                Models.Army Defender = new Models.Army();
                Attacker.Units.Add(new Unit(UnitStats.AllUnits[1]));
                Defender.Units.Add(new Unit(UnitStats.AllUnits[1]));

                Models.Modifier.BattleSolver.Solve(ref Attacker, ref Attacker);
            }

            return Content(Models.Modifier.BattleSolver.test);
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
    }
}
