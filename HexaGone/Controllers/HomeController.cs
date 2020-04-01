using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using HexaGone.Models;
using System.Net;
using System.Data;
using Dapper;
using MySql.Data.MySqlClient;
using Dapper.Contrib;
using Dapper.Contrib.Extensions;

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
            return View(new UserModel() { isLogin = "true" }); 
        }

        [HttpPost]
        public IActionResult Index(HexaGone.Models.UserModel user)
        {
            if (user.isLogin == "false")
            {
                user.LoginModel = null;
            }
            if (ModelState.IsValid)
            {
                if (user.isLogin == "true")
                {
                    return Content(user.LoginModel.Password);
                }
                else
                {
                    return Content(user.RegistrationModel.Password);
                }
            }
            else
            {
                return View(new UserModel() { isLogin = "true" });
            }
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
        [HttpPost]
        public IActionResult Register(HexaGone.Models.UserModel user)
        {
            using (IDbConnection db = new MySqlConnection(Models.Dapper.connectionString))
            {
                
                string sqlQuery = "Insert Into User (Email, Username, Password) Values(@Email, @Username, @Password)";
                user.RegistrationModel.Password = Hash.GetMD5Hash(user.RegistrationModel.Password);
                
                int rowsAffected = db.Execute(sqlQuery, user.RegistrationModel);

            }
            return Content("true");
        }
    }
}
