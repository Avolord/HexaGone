﻿using System;
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
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Game()
        {
            Models.Hexmap hexmap = new Models.Hexmap();

            //===
            // Ausfüllen:
            hexmap.hexSideLength = 30;
            hexmap.width = hexmap.mapData[0].Count;
            hexmap.height = hexmap.mapData.Count;
            hexmap.isPointy = false;
            //===

            hexmap.texture_index = new int[hexmap.width][];

            for (int i = 0; i < hexmap.width; i++)
            {
                hexmap.texture_index[i] = new int[hexmap.height];
            }

            for (int i = 0; i < hexmap.width; i++)
            {
                for (int j = 0; j < hexmap.height; j++)
                {
                    Random rand = new Random();
                    hexmap.texture_index[i][j] = rand.Next(0, 30);
                }
            }

            hexmap.calculate();

            return View("Game", hexmap);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
