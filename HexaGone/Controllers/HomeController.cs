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
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Http;

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
            if (HttpContext.Request.Cookies.ContainsKey("stayLoggedIn"))
            {
                var userId = HttpContext.Request.Cookies["stayLoggedIn"];
                return Content(userId.ToString());
            }
                return View(new UserModel() { isLogin = "true", errorMessage = "" }) ; 
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
            user.isLogin = "false";
            if (user.isValid())
            {
                user.errorMessage = "";
                using (IDbConnection db = new MySqlConnection(Models.Dapper.connectionString))
                {
                    user.RegistrationModel.Email = user.RegistrationModel.Email.ToLower();
                    string sqlQuery = "Select * From User Where Username = \"" + user.RegistrationModel.Username + "\"";
                    string sqlQuery2 = "Select * From User Where Email =  \"" + user.RegistrationModel.Email + "\"";
                    List<LoginUserModel> witzigerName = new List<LoginUserModel>();
                
                        try
                        {
                            witzigerName = db.Query<LoginUserModel>(sqlQuery).ToList();
                            if(witzigerName.Count != 0)
                            {
                                user.errorMessage = "Username already exists";
                            }
                        }
                        finally
                        {
                        }
                        if (witzigerName.Count == 0)
                        {
                            try
                            {
                                witzigerName = db.Query<LoginUserModel>(sqlQuery2).ToList();
                                if(witzigerName.Count != 0)
                                {
                                    user.errorMessage = "Email already exists";
                                }
                            }
                            finally
                            {
                            }
                        }
                        if (witzigerName.Count != 0)
                        {
                            user.isLogin = "false";
                            
                            return View("Index", user);
                        }
                        else
                        {
                        }

                        sqlQuery = "Insert Into User (Email, Username, Password) Values(@Email, @Username, @Password)";
                        user.RegistrationModel.Password = Hash.GetMD5Hash(user.RegistrationModel.Password);
                        
                        int rowsAffected = db.Execute(sqlQuery, user.RegistrationModel);
                    user.isLogin = "false";
                    return View("Index",user);
               
                }
            }
            else
            {
                user.isLogin = "false";
                user.errorMessage = user.RegistrationModel.IsValid();
                
                return View("Index",user);
            }

        }
        [HttpPost]
        public IActionResult Login(HexaGone.Models.UserModel user)
        {
            user.isLogin = "true";
            if (user.isValid())
            {
                user.errorMessage = "";
                using (IDbConnection db = new MySqlConnection(Models.Dapper.connectionString))
                {
                    if (user.LoginModel.Username.Contains("@"))
                    {
                        user.LoginModel.Email = user.LoginModel.Username.ToLower();
                    }

                    user.LoginModel.Password = Hash.GetMD5Hash(user.LoginModel.Password);

                    string sqlQuery = "Select * From User Where Username = \"" + user.LoginModel.Username + "\"";
                    string sqlQuery2 = "Select * From User Where Email =  \"" + user.LoginModel.Email + "\"";
                    List<LoginUserModel> witzigerName = new List<LoginUserModel>();
                    bool currentUserName = false;
                    bool currentUserMail = false;
                    int indexOfUserInList = -1;
                    try
                    {
                        witzigerName = db.Query<LoginUserModel>(sqlQuery).ToList();
                        if(witzigerName.Count == 0)
                        {
                            user.errorMessage = "User doesn't exists";
                        }
                        else
                        {
                            currentUserName = true;
                        }

                    }
                    finally
                    {
                    }
                    if (witzigerName.Count == 0)
                    {
                        try
                        {
                            witzigerName = db.Query<LoginUserModel>(sqlQuery2).ToList();
                            if(witzigerName.Count == 0 && user.LoginModel.Username.Contains("@"))
                            {
                                user.errorMessage = "Email doesn't exists";
                            }
                            else if(witzigerName.Count != 0)
                            {
                                currentUserMail = false;
                            }
                        }
                        finally
                        {
                        }
                    }
                    if (witzigerName.Count == 0)
                    {
                        user.isLogin = "true";
                        return View("Index",user);
                    }
                    else
                    {
                        for (int i = 0; i<witzigerName.Count; i++) 
                        {
                            var Item = witzigerName[i];
                            if (user.LoginModel.Password == Item.Password)
                            {
                                user.isLogin = "true";
                                if(user.stayLoggedIn)
                                {
                                    if (currentUserName == true)
                                    {
                                        sqlQuery = "Select UserID From User Where Username = \"" + user.LoginModel.Username + "\"";
                                    }
                                    else
                                    {
                                        sqlQuery = "Select UserID From User Where Email = \"" + user.LoginModel.Email + "\"";
                                    }
                                    CookieOptions stayLoggedIn = new CookieOptions();
                                    stayLoggedIn.Expires = new DateTimeOffset(DateTime.Now.AddSeconds(100));
                                    List<int> userId = new List<int>();
                                    userId = db.Query<int>(sqlQuery).ToList();
                                    user.errorMessage = "";
                                    HttpContext.Response.Cookies.Append("stayLoggedIn", userId[0].ToString(), stayLoggedIn);
                                }
                               
                                return View("Index",user);
                            }
                        }
                    }

                }
                user.isLogin = "true";
                user.errorMessage = "Password is incorrect";
                return View("Index", user);
            }
            else
            {
                user.isLogin = "true";
                user.errorMessage = user.LoginModel.IsValid();
                return View("Index",user);
            }
        }
    }
}
