using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using WebApplication.Repository;
using WebApplication.ViewModels.AcccountViewModel;

namespace WebApplication.Controllers
{
    public class AccountController : Controller
    {
        private UserManager<IdentityUser> _userManager;
        private SignInManager<IdentityUser> _signInManager;
        private ImessageSender _messageSender;

        public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, ImessageSender messageSender)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _messageSender = messageSender;
        }



        [HttpGet]
        public IActionResult Register()
        {
            if (_signInManager.IsSignedIn(User))
                return RedirectToAction("Index", "Home");

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (_signInManager.IsSignedIn(User))
                return RedirectToAction("Index", "Home");
            if (ModelState.IsValid)
            {
                var user = new IdentityUser()
                {

                    UserName = model.UserName,
                    Email = model.Email,


                };

                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    var emailConfigurationToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    var emailMessage = Url.Action
                    ("ConfirmEmail",
                        "Account"
                        , new { username = user.UserName, token = emailConfigurationToken }
                        , Request.Scheme);
                    await _messageSender.SendEmailAsync(model.Email, "Email confirmation from zarrab", emailMessage);
                    return RedirectToAction("Index", "Home");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(model);
        }
        [HttpGet]

        public async Task<IActionResult> Login(string returnUrl = null)
        {
            if (_signInManager.IsSignedIn(User))
            {
                return RedirectToAction("Index", "Home");
            }

            #region forgoogle login

            var model = new LoginViweModel()
            {
                ReturnUrl = returnUrl,
                ExternalLogin =( await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList()
            };

            #endregion

            ViewData["returnUrl"] = returnUrl;
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViweModel model, string returnUrl = null)
        {
            model.ReturnUrl = returnUrl;
            model.ExternalLogin = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid)
            {
                var user = _userManager.FindByEmailAsync(model.Email);
                if (user.Result != null)
                {

                    ViewData["returnUrl"] = returnUrl;

                    var result = await _signInManager.PasswordSignInAsync(
                        user.Result.UserName,
                        model.Password,
                        model.Rememberme,
                        true);
                    if (result.Succeeded)
                    {
                        ViewBag.IsSuccess = true;
                        //baraye amniyat
                        if (string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))

                        {
                            return Redirect(returnUrl);

                        }



                        return View(model);
                    }

                    if (result.IsLockedOut)
                    {

                        ViewData["ErorrMessage"] = "اکانت شما به دلیل 5 بار ورود ناموفق قفل شده است ";
                        return View(model);
                    }
                }

                ModelState.AddModelError("", "رمز عبور یا نام کاربری اشتباه است ");

            }


            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> IsEmailInUse(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                return Json(true);
            return Json("ایمیل وارد شده موجود است ");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> IsUserInUse(string userName)
        {
            var user = await _userManager.FindByNameAsync(userName);
            if (user == null)
                return Json(true);
            return Json("نام کاربری وارد شده موجود است ");
        }

        [HttpGet]
        public async Task<IActionResult> ConfirmEmail(string userName, string token)
        {
            if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(token))
                return NotFound();
            var user = await _userManager.FindByNameAsync(userName);
            if (user == null) return NotFound();
            var result = await _userManager.ConfirmEmailAsync(user, token);
            return Content(result.Succeeded ? "Email confirmed" : "Email not confirm");
        }

        public IActionResult ForgetPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgetPassword(ForgetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user != null)
                {
                    var passwordConfigurationToken = await _userManager.GeneratePasswordResetTokenAsync(user);
                    var emailMessage = Url.Action
                    ("ResetPassword",
                        "Account"
                        , new { email = user.Email, token = passwordConfigurationToken }
                        , Request.Scheme);
                    await _messageSender.SendEmailAsync(model.Email, "Reset Passwor Link", emailMessage);
                    ViewBag.IsSuccess = true;
                }



            }

            return View(model);
        }

        public IActionResult ResetPassword(string email, string token)
        {
            var model = new ResetPasswordViewModel()
            {
                Token = token,
                Email = email,

            };
            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user == null)
                {
                    return View(model);
                }

                var resetPasswordResult = await _userManager.ResetPasswordAsync(user, model.Token, model.Password);
                if (!resetPasswordResult.Succeeded)
                {
                    foreach (var error in resetPasswordResult.Errors)
                    {
                        ModelState.TryAddModelError(error.Code, error.Description);
                    }
                }

                ViewBag.IsSuccess = true;
            }

            return View(model);
        }

        [HttpPost]
        public IActionResult ExternalLogin(string provider, string returnUrl)
        {
            var redirectUrl = Url.Action("ExternalLoginCallBack", "Account",
                new { ReturnUrl = returnUrl });

            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            return new ChallengeResult(provider, properties);
        }

        public async Task<IActionResult> ExternalLoginCallBack(string returnUrl = null, string remoteError = null)
        {
            returnUrl =
                (returnUrl != null && Url.IsLocalUrl(returnUrl)) ? returnUrl : Url.Content("~/");

            var loginViewModel = new LoginViweModel()
            {
                ReturnUrl = returnUrl,
               ExternalLogin = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList()
            };

            if (remoteError != null)
            {
                ModelState.AddModelError("", $"Error : {remoteError}");
                return View("Login", loginViewModel);
            }

            var externalLoginInfo = await _signInManager.GetExternalLoginInfoAsync();
            if (externalLoginInfo == null)
            {
                ModelState.AddModelError("ErrorLoadingExternalLoginInfo", $"مشکلی پیش آمد");
                return View("Login", loginViewModel);
            }

            var signInResult = await _signInManager.ExternalLoginSignInAsync(externalLoginInfo.LoginProvider,
                externalLoginInfo.ProviderKey, false, true);

            if (signInResult.Succeeded)
            {
                return Redirect(returnUrl);
            }

            var email = externalLoginInfo.Principal.FindFirstValue(ClaimTypes.Email);

            if (email != null)
            {
                var user = await _userManager.FindByEmailAsync(email);
                if (user == null)
                {
                    var userName = email.Split('@')[0];
                    user = new IdentityUser()
                    {
                        UserName = (userName.Length <= 10 ? userName : userName.Substring(0, 10)),
                        Email = email,
                        EmailConfirmed = true
                    };

                    await _userManager.CreateAsync(user);
                }

                await _userManager.AddLoginAsync(user, externalLoginInfo);
                await _signInManager.SignInAsync(user, false);

                return Redirect(returnUrl);
            }

            ViewBag.ErrorTitle = "لطفا با بخش پشتیبانی تماس بگیرید";
            ViewBag.ErrorMessage = $"دریافت کرد {externalLoginInfo.LoginProvider} نمیتوان اطلاعاتی از";
            return View();
        }


    }
}
