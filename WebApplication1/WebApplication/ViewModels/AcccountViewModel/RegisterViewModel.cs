using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication.ViewModels.AcccountViewModel
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "نام کاربری الزامی است ")]
        [MaxLength(15)]
        [Display(Name = "نام کاربری")]
        //[Remote("IsUserInUse","Account")] baraye afzayesh amniyat be shekle zir minevisim
        [Remote("IsUserInUse", "Account",HttpMethod = "Post",AdditionalFields = "__RequestVerificationToken")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "ایمیل الزامی است ")]
        [Display(Name = "ایمیل")]
        [EmailAddress]
        [Remote("IsEmailInUse", "Account",HttpMethod = "Post", AdditionalFields = "__RequestVerificationToken")]
        public string Email { get; set; }

        [Required]
        [MaxLength(20)]
        [Display(Name = "رمز")]
        [DataType(DataType.Password)]

        public string Password { get; set; }

        [Required]
        [MaxLength(20)]
        [Display(Name = "تکرار رمز")]
        [Compare("Password")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }    
    }
}