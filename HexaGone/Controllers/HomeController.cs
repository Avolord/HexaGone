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
using System.Collections.Immutable;


namespace HexaGone.Controllers
{
    public class HomeController : Controller
    {
        
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Tries to logIn the User via Cookie. First Page which is opened when website is opened via homedirectory
        /// </summary>
        /// <returns>LogedInIndex Page or IndexPage</returns>
        public IActionResult Index()
        {
            //Verifying wether the user is logged in or not
            string sessionData = HttpContext.Session.GetString("userKeyData");

            //if: User is logged In. Forwarding to logged in Page
            //elseIf: user checked stay logged in. Cookie exists: Forwarding to LoggedInPage
            if (sessionData != null && sessionData.Length > 0)
            {
                UserModel currentUser = new UserModel();
                currentUser.ReadSessionCookieData(sessionData);
                //return LogedInIndex with existing User
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
                HttpContext.Session.SetString("userKeyData", userLoaded[0].CreateSessionString());
                //return LogedInIndex with existing User
                return View("LogedInIndex", userLoaded[0]);
            }
            //return Index Page with new UserModel
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
                    HttpContext.Session.SetString("userKeyData", userLoaded[0].CreateSessionString());
                }
            }
            //Is the User logged in? True:: Go to Game False::redirect to LogInPage
            if (sessionData != null && sessionData.Length > 0)
            {
                Models.Map map = new Map(Map.mapModeBiomes, Map.Big, Map.Small);

                //===
                // Ausfüllen:
                map.HexSideLength = 30;
                //===

                //user is logged In redirect to game
                return View("Game", map);
            }
            else
            {
                //User is not logged in redirect to LoginPage
                return View("Index",new UserLoginHelperModel() { isLogin = "true", errorMessage = "Please login to visit the Game page", redirectTo = "Game" });
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
       
        /// <summary>
        /// Handles the registrationPostRequest.
        /// </summary>
        /// <param name="user">An UserModel</param>
        /// <returns>LoggedInIndexPage or indexPage with registration Form opened with ErrorMessages</returns>
        [HttpPost]
        public IActionResult Register(HexaGone.Models.UserLoginHelperModel user)
        {
            user.isLogin = "false";
            //If given Data is correct. Try to create User in database. First verify if an user with similar data already exists.
            //Else return errorMessage
            if (user.isValid())
            {
                user.errorMessage = "";
                using (IDbConnection db = new MySqlConnection(Models.Dapper.connectionString))
                {
                    user.RegistrationModel.Email = user.RegistrationModel.Email.ToLower();
                    string sqlQuery = "Select * From User Where Username = \"" + user.RegistrationModel.Username + "\"";
                    string sqlQuery2 = "Select * From User Where Email =  \"" + user.RegistrationModel.Email + "\"";
                    List<LoginUserModel> possibleUser = new List<LoginUserModel>();
                        //First try if the Username exists and give back an error via errorMessage
                        try
                        {
                            possibleUser = db.Query<LoginUserModel>(sqlQuery).ToList();
                            if(possibleUser.Count != 0)
                            {
                                user.errorMessage = "Username already exists";
                            }
                        }
                        finally
                        {
                        }
                        //If the Username doesn't exists, try to find a same email adress in database
                        if (possibleUser.Count == 0)
                        {
                            try
                            {
                                possibleUser = db.Query<LoginUserModel>(sqlQuery2).ToList();
                                if(possibleUser.Count != 0)
                                {
                                //Email exists, give back an error via errorMessage
                                    user.errorMessage = "Email already exists";
                                }
                            }
                            finally
                            {
                            }
                        }
                        if (possibleUser.Count != 0)
                        {
                            //Something went really wrong redirect to Index... Shouldn't get to this point and doesn't, as long as the database is up and running
                            user.isLogin = "false";
                            
                            return View("Index", user);
                        }
                        else
                        {
                        }
                        //Create sqlCommand and encrypt password
                        sqlQuery = "Insert Into User (Email, Username, Password) Values(@Email, @Username, @Password)";
                        user.RegistrationModel.Password = Hash.GetMD5Hash(user.RegistrationModel.Password);
                        
                        //Create new User in database
                        int rowsAffected = db.Execute(sqlQuery, user.RegistrationModel);

                        //if Creation was done right create Session cookie
                        if(rowsAffected > 0)
                        {
                            List<UserModel> userLoaded;
                            sqlQuery = "Select * From User Where Username = \"" + user.RegistrationModel.Username + "\"";
                            userLoaded = db.Query<UserModel>(sqlQuery).ToList();
                            HttpContext.Session.SetString("userKeyData", userLoaded[0].CreateSessionString());
                            //return LoggedInIndex-Page
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
                //return registration page, because RegistrationModel was not Valid, with good Errormessages
                return View("Index",user);
            }

        }
        
        
        /// <summary>
        /// Handles the Post logIn request
        /// </summary>
        /// <param name="user">An User model</param>
        /// <returns>User data correct: LogedInPage oder a restricted Page\nUser data incorrect IndexPage with error</returns>
        [HttpPost]
        public IActionResult Login(HexaGone.Models.UserLoginHelperModel user)
        {
            user.isLogin = "true";
            //If userModel is Valid try to login the User
            //Else return a good errorMessage
            if (user.isValid())
            {
                user.errorMessage = "";
                using (IDbConnection db = new MySqlConnection(Models.Dapper.connectionString))
                {
                    //Correct Email format to all lower Case
                    if (user.LoginModel.Username.Contains("@"))
                    {
                        user.LoginModel.Email = user.LoginModel.Username.ToLower();
                    }
                    //Encrypt Password
                    user.LoginModel.Password = Hash.GetMD5Hash(user.LoginModel.Password);

                    //Create both sqlQueryCommands for Email and Username to test if User exists
                    string sqlQueryUsername = "Select * From User Where Username = \"" + user.LoginModel.Username + "\"";
                    string sqlQueryEmail = "Select * From User Where Email =  \"" + user.LoginModel.Email + "\"";
                    List<LoginUserModel> possibleUser;
                    bool currentUserName = false;
                    bool currentUserMail = false;

                    //First test for UserName
                    try
                    {
                        possibleUser = db.Query<LoginUserModel>(sqlQueryUsername).ToList();
                        if(possibleUser.Count == 0)
                        {
                            //If Username doesn't exist set errorMessage, doesn't has to mean. That the User won't be logged in. Reason for Error can be, that Username is Email
                            user.errorMessage = "User doesn't exists";
                        }
                        else
                        {
                            //Set that the UserName is an Username and not an Email
                            currentUserName = true;
                        }

                    }
                    finally
                    {
                    }
                    //Second test for Email only if Username conatins an @
                    if (possibleUser.Count == 0)
                    {
                        try
                        {
                            possibleUser = db.Query<LoginUserModel>(sqlQueryEmail).ToList();
                            if(possibleUser.Count == 0 && user.LoginModel.Username.Contains("@"))
                            {
                                //return errorMessage that the User doesn't exists. Means that the User won't be Logged In
                                user.errorMessage = "Email doesn't exists";
                            }
                            else if(possibleUser.Count != 0 && user.LoginModel.Username.Contains("@"))
                            {
                                //Set that the UserName is an Email
                                currentUserMail = true;
                                currentUserName = false;
                            }
                        }
                        finally
                        {
                        }
                    }
                    //If true: return IndexPage with error
                    if (possibleUser.Count == 0)
                    {
                        //If User doesn't exists. Return LogInPage with errors
                        user.isLogin = "true";
                        return View("Index",user);
                    }
                    //Try to LogIn the User with encrypted Password
                    else
                    {
                        //Try to LogIn the User. For loop wouldn't be needed, because  every user exists only once,however if an error occurs it is less likely to crash
                        for (int i = 0; i<possibleUser.Count; i++) 
                        {
                            //Compare Passwords if passwords are equal log in the User by getting data from the database and create the SessionCookie
                            var Item = possibleUser[i];
                            if (user.LoginModel.Password == Item.Password)
                            {
                                user.isLogin = "true";
                                string sqlQuery = "";
                                List<UserModel> userLoaded;
                                //If User logs in by Username get data by Username
                                //Else get data by Email
                                if (currentUserName)
                                    
                                    sqlQuery = "Select * From User Where Username = \"" + user.LoginModel.Username + "\"";
                                else if (currentUserMail)
                                    sqlQuery = "Select * From User Where Email = \"" + user.LoginModel.Email + "\"";
                               
                                userLoaded = db.Query<UserModel>(sqlQuery).ToList();
                                HttpContext.Session.SetString("userKeyData", userLoaded[0].CreateSessionString());
                                
                                //If User wants to stay Logged In create Cookie
                                if (user.stayLoggedIn)
                                {
                                    //Create Cookie
                                    CookieOptions stayLoggedIn = new CookieOptions();
                                    stayLoggedIn.Expires = new DateTimeOffset(DateTime.Now.AddYears(100));
                                    user.errorMessage = "";

                                    //Safe Cookie
                                    HttpContext.Response.Cookies.Append("stayLoggedIn", userLoaded[0].UserId.ToString(), stayLoggedIn);
                                }

                                //If User comes from a restricted page redirect back to it
                                
                                if(String.IsNullOrEmpty(user.redirectTo))
                                {
                                    if(user.redirectTo == "Game")
                                    {
                                        return Game();
                                    }
                                }
                                //else redirect back to  LogedInIndex Page
                                return View("LogedInIndex", userLoaded[0]);
                            }
                        }
                    }

                }
                //If User couldn't be Logged In because of wrong Password return IndexPage with errorMessage
                user.isLogin = "true";
                user.errorMessage = "Password is incorrect";
                return View("Index", user);
            }
            else
            {
                user.isLogin = "true";
                user.errorMessage = user.LoginModel.IsValid();
                //return Login Page with ErrorMessage added
                return View("Index",user);
            }
        }

        /// <summary>
        /// remove all Cookiedata, then logout the User
        /// </summary>
        /// <returns>Indexview with the new Usermodel </returns>
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("userKeyData");
            HttpContext.Response.Cookies.Delete("stayLoggedIn");
            return View("Index", new UserLoginHelperModel() { isLogin = "true", errorMessage = "" });
        }
    }
}
