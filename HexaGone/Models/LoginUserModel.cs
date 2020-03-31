using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace HexaGone.Models
{
    public class LoginUserModel
    {
        [Required]
        [MinLength(4)]
        public string Username { get; set; }
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public bool IsValid()
        {
            if(Username.Length < 4)
            {
                return false;
            }
            if(!(Email.Contains("@") && Email.Contains(".") && Email.Length > 7))
            {
                return false;
            }
            if(Password.Length < 2)
            {
                return false;
            }
            return true;
        }
    }
}
