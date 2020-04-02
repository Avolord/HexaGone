using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace HexaGone.Models
{
    public class RegistrationUserModel
    {
        [Range(4, 12)]
        [Required]
        public string Username { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        [Compare("Password")]
        [DataType(DataType.Password)]
        public string RepeatPassword { get; set; }



        public string IsValid()
        {
            if (Password == null)
            {
                return "Please enter a password";
            }
            if (RepeatPassword == null)
            {
                return "Please enter a password";
            }
            if (Email == null)
            {
                return "Please enter Email";
            }
            if(Username == null)
            {
                return "Please enter Username";
            }
            if (Username.Length < 4)
            {
                return "Username must be at least 4 characters long";
            }
            if (!(Email.Contains("@") && Email.Contains(".") && Email.Length > 7))
            {
                return "Not an Email";
            }
            if (Password.Length < 2)
            {
                return "Password to short";
            }
            if (RepeatPassword != Password)
            {
                return "Passwords are not equal";
            }
            return "true";
        }

    }
}
