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
            string sessionData = HttpContext.Session.GetString("userKeyData");
            if (sessionData != null && sessionData.Length > 0)
            {
                UserModel currentUser = new UserModel();
                currentUser.readSessionCookieData(sessionData);
                return View("LogedInIndex", currentUser);
            }
            else if (HttpContext.Request.Cookies.ContainsKey("stayLoggedIn"))
            {
                var userId = HttpContext.Request.Cookies["stayLoggedIn"];
                List<UserModel> userLoaded = new List<UserModel>();
                string sqlQueryUserId = "Select * From User Where UserID = \"" + userId + "\"";
                using (IDbConnection db = new MySqlConnection(Models.Dapper.connectionString))
                {
                    userLoaded = db.Query<UserModel>(sqlQueryUserId).ToList();
                }
                HttpContext.Session.SetString("userKeyData", userLoaded[0].createSessionString());
                return View("LogedInIndex", userLoaded[0]);
            }
           
            return View(new UserLoginHelperModel() { isLogin = "true", errorMessage = "" });
            
        }

        [HttpPost]
        public IActionResult Index(HexaGone.Models.UserLoginHelperModel user)
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
                return View(new UserLoginHelperModel() { isLogin = "true" });
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Game()
        {
            string sessionData = HttpContext.Session.GetString("userKeyData");
            if (sessionData == null || sessionData.Length == 0)
            {
                if (HttpContext.Request.Cookies.ContainsKey("stayLoggedIn"))
                {
                    var userId = HttpContext.Request.Cookies["stayLoggedIn"];
                    List<UserModel> userLoaded = new List<UserModel>();
                    string sqlQueryUserId = "Select * From User Where UserID = \"" + userId + "\"";
                    using (IDbConnection db = new MySqlConnection(Models.Dapper.connectionString))
                    {
                        userLoaded = db.Query<UserModel>(sqlQueryUserId).ToList();
                    }
                    HttpContext.Session.SetString("userKeyData", userLoaded[0].createSessionString());
                }
            }
            if (sessionData != null && sessionData.Length > 0)
            {
                Models.Hexmap hexmap = new Models.Hexmap();

                //===
                // Ausfüllen:
                hexmap.hexSideLength = 30;
                hexmap.width = 20;
                hexmap.height = 30;
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
            else
            {
                return View("Index",new UserLoginHelperModel() { isLogin = "true", errorMessage = "Please login to visit the Game page", redirectTo = "Game" });
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        [HttpPost]
        public IActionResult Register(HexaGone.Models.UserLoginHelperModel user)
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
                        if(rowsAffected > 0)
                        {
                            List<UserModel> userLoaded = new List<UserModel>();
                            sqlQuery = "Select * From User Where Username = \"" + user.RegistrationModel.Username + "\"";
                            userLoaded = db.Query<UserModel>(sqlQuery).ToList();
                            HttpContext.Session.SetString("userKeyData", userLoaded[0].createSessionString());
                            return View("LogedInIndex", userLoaded[0]);
                        }
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
        public IActionResult Login(HexaGone.Models.UserLoginHelperModel user)
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

                    string sqlQueryUsername = "Select * From User Where Username = \"" + user.LoginModel.Username + "\"";
                    string sqlQueryEmail = "Select * From User Where Email =  \"" + user.LoginModel.Email + "\"";
                    List<LoginUserModel> witzigerName = new List<LoginUserModel>();
                    bool currentUserName = false;
                    bool currentUserMail = false;
                    try
                    {
                        witzigerName = db.Query<LoginUserModel>(sqlQueryUsername).ToList();
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
                            witzigerName = db.Query<LoginUserModel>(sqlQueryEmail).ToList();
                            if(witzigerName.Count == 0 && user.LoginModel.Username.Contains("@"))
                            {
                                user.errorMessage = "Email doesn't exists";
                            }
                            else if(witzigerName.Count != 0 && user.LoginModel.Username.Contains("@"))
                            {
                                currentUserMail = true;
                                currentUserName = false;
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
                                string sqlQuery = "";
                                List<UserModel> userLoaded = new List<UserModel>();
                                if (currentUserName)
                                    sqlQuery = "Select * From User Where Username = \"" + user.LoginModel.Username + "\"";
                                else if (currentUserMail)
                                    sqlQuery = "Select * From User Where Email = \"" + user.LoginModel.Email + "\"";
                                userLoaded = db.Query<UserModel>(sqlQuery).ToList();
                                HttpContext.Session.SetString("userKeyData", userLoaded[0].createSessionString());
                                if (user.stayLoggedIn)
                                {
                                    CookieOptions stayLoggedIn = new CookieOptions();
                                    stayLoggedIn.Expires = new DateTimeOffset(DateTime.Now.AddSeconds(100));
                                    user.errorMessage = "";

                                    HttpContext.Response.Cookies.Append("stayLoggedIn", userLoaded[0].userId.ToString(), stayLoggedIn);
                                }
                                if(user.redirectTo != null && user.redirectTo != "")
                                {
                                    if(user.redirectTo == "Game")
                                    {
                                        return Game();
                                    }
                                }
                                return View("LogedInIndex", userLoaded[0]);
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
