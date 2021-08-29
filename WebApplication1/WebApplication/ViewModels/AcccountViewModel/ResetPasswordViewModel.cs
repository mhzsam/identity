using System.ComponentModel.DataAnnotations;

namespace WebApplication.ViewModels.AcccountViewModel
{
    public class ResetPasswordViewModel
    {
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

        public string Email { get; set; }
        public string Token { get; set; }
    }
}