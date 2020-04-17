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

        /// <summary>
        /// tries to validate the User Model
        /// </summary>
        /// <returns>errorMessage or String 'true'</returns>
        public string IsValid()
        {
            if(Password == null)
            {
                return "Please enter a password";
            }
            if(Email  == null && Username == null)
            {
                return "Please enter Email or Username";
            }
            if ( Username != null && Username.Length < 4 && !Username.Contains("@"))
            {
                return "Username must be at least 4 characters long";
            }            
            if (Email != null && !(Email.Contains("@") && Email.Contains(".") && Email.Length > 7))
            {
                return "Not an Email";
            }
            if(Password.Length < 2)
            {
                return "Password to short";
            }
            if (Username != null && !SqlHelper.IsStringValid(Username))
            {
                return "Username not allowed";
            }
            if (!SqlHelper.IsStringValid(Password))
            {
                return "Password not allowed";
            }
            if (Email != null && !SqlHelper.IsStringValid(Email))
            {
                return "Email not allowed";
            }

            return "true";
        }
    }
}
