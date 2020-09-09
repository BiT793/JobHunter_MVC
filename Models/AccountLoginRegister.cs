using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace JobHunter_MVC.Models
{
    public class AccountLoginRegister
    {
        public class UserLogin
        {
            [Required]
            public string login { get; set; }

            [Required]
            [DataType(DataType.Password)]
            public string password { get; set; }
        }

        public class UserRegister
        {

            [Required]
            public string login { get; set; }

            [Required]
            [DataType(DataType.Password)]
            public string password { get; set; }
            [Required]
            [DataType(DataType.EmailAddress, ErrorMessage = "E-mail is not valid")]
            public string Email { get; set; }
            [Required]
            [DataType(DataType.Password)]
            [Compare("password", ErrorMessage = "Введены несовпадающие пороли, повторите попытку.")]
            public string ConfirmPassword { get; set; }
            [Required] public string company { get; set; }
            [Required] public string access_level { get; set; }
            [Required] public DateTime create_date { get; set; }
        }
    }
}