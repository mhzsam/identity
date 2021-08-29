using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authentication;

namespace WebApplication.ViewModels.AcccountViewModel
{
    public class LoginViweModel
    {
        [Required,Display(Name = "نام کاربری")]
        public string Email { get; set; }


        [Required, Display(Name = "پسورد")]
        [DataType(DataType.Password)]
        public string Password { get; set; }  


        [ Display(Name = "مرا به خاطر بسپار")]
        public bool Rememberme { get; set; }

        public string ReturnUrl { get; set; }
        public List<AuthenticationScheme> ExternalLogin { get; set; }

    }
}