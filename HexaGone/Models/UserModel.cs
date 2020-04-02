using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HexaGone.Models
{
    public class UserModel
    {
        public LoginUserModel LoginModel { get; set; }
        public RegistrationUserModel RegistrationModel { get; set; }
        public bool LoginIsValid { get; set; }
        public bool Registra6tionIsValid { get; set; }

        public bool stayLoggedIn { get; set; }

        public string isLogin { get; set; }

        public string errorMessage { get; set; }


        public bool isValid()
        {
            if (isLogin == "true")
            {
                if (LoginModel.IsValid() == "true")
                    return true;
            }
            else
            {
                if (RegistrationModel.IsValid() == "true")
                {
                    return true;
                }
            }
            return false;
        }

    }
}
