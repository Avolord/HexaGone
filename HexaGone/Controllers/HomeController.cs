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
            if (ModelState.IsValid)
            {
                using (IDbConnection db = new MySqlConnection(Models.Dapper.connectionString))
                {
                    user.RegistrationModel.Email = user.RegistrationModel.Email.ToLower();
                    string sqlQuery = "Select * From User Where Username = \"" + user.RegistrationModel.Username + "\"";
                    string sqlQuery2 = "Select * From User Where Email =  \"" + user.RegistrationModel.Email + "\"";
                    List<LoginUserModel> witzigerName = new List<LoginUserModel>();
                
                        try
                        {
                            witzigerName = db.Query<LoginUserModel>(sqlQuery).ToList();
                        }
                        finally
                        {
                        }
                        if (witzigerName.Count == 0)
                        {
                            try
                            {
                                witzigerName = db.Query<LoginUserModel>(sqlQuery2).ToList();
                            }
                            finally
                            {
                            }
                        }
                        if (witzigerName.Count != 0)
                        {
                            return Content("false");
                        }
                        else
                        {
                        }

                        sqlQuery = "Insert Into User (Email, Username, Password) Values(@Email, @Username, @Password)";
                        user.RegistrationModel.Password = Hash.GetMD5Hash(user.RegistrationModel.Password);
                        
                        int rowsAffected = db.Execute(sqlQuery, user.RegistrationModel);
               
                }
            }
            else
            {
                return View(user);
            }

        }
        [HttpPost]
        public IActionResult Login(HexaGone.Models.UserModel user)
        {
            using (IDbConnection db = new MySqlConnection(Models.Dapper.connectionString))
            {
                if(user.LoginModel.Username.Contains("@"))
                {
                    user.LoginModel.Email = user.LoginModel.Username.ToLower();
                }

                user.LoginModel.Password = Hash.GetMD5Hash(user.LoginModel.Password);
                
                string sqlQuery = "Select * From User Where Username = \""+user.LoginModel.Username+"\"";
                string sqlQuery2 = "Select * From User Where Email =  \"" + user.LoginModel.Email + "\"";
                List<LoginUserModel> witzigerName = new List<LoginUserModel>();

                try
                {
                    witzigerName = db.Query<LoginUserModel>(sqlQuery).ToList();
                }
                finally
                {
                }
                if(witzigerName.Count == 0)
                {
                    try
                    {
                        witzigerName = db.Query<LoginUserModel>(sqlQuery2).ToList();
                    }
                    finally
                    {
                    }
                }
                if(witzigerName.Count == 0)
                {
                    return Content("false");
                }
                else
                {
                    foreach(var Item in witzigerName)
                    {
                        if(user.LoginModel.Password == Item.Password)
                        {
                            return Content("true");
                        }
                    }
                }


            }
            return Content("false");
        }
    }
}
