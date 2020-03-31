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
        public bool RegistrationIsValid { get; set; }

        public string isLogin { get; set; }


    }
}
