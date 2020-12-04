using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExamExpert.Common.ViewModels;
using ExamExpert.Data.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ExamExpert.WebApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly ILogger _logger;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager, ILogger<AccountController> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
        }

        [TempData]
        public string ErrorMessage { get; set; }


        #region Register
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterVM registerVM)
        {

            if (ModelState.IsValid)
            {
                var user = new User { UserName = registerVM.UserName, Email = registerVM.Email, CreateDate = DateTime.Now };
                var result = await _userManager.CreateAsync(user, registerVM.Password);
                if (result.Succeeded)
                {
                    var addRoleResult = _userManager.AddToRoleAsync(user, "Teacher").Result;
                    if (addRoleResult.Succeeded)
                    {
                        _logger.LogInformation("Kullanıcı başarıyla oluştu.");

                        //var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                        //code = HttpUtility.UrlEncode(code);
                        //var callbackUrl =
                        //    string.Concat(Request.Scheme, "://", Request.Host, Url.Action("ConfirmEmail", "Account", null), "?code=", code, "&userId=", user.Id);

                        //await _emailSender.SendEmailConfirmationAsync(model.Email, callbackUrl);

                        await _signInManager.SignInAsync(user, isPersistent: false);
                        _logger.LogInformation("Kullanıcı başarıyla oluştu.");
                        //return Redirect(callbackUrl);
                        return Redirect(string.Concat(Request.Scheme, "://", Request.Host, Url.Action("Index", "Home", null), "?userId=", user.Id));
                    }
                    else
                    {
                        AddErrors(result);
                    }
                }
                AddErrors(result);
            }

            // sorun olursa tekrar ekran render olur
            return View(registerVM);
        }
        #endregion

        #region Login
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Login()
        {
            // Daha önceden var olan cookie temizlenir, başarılı bir giriş yapılabilmesi için.
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginVM loginVM)
        {

            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(loginVM.UserName);

                var result = await _signInManager.PasswordSignInAsync(user.UserName, loginVM.Password, loginVM.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    _logger.LogInformation("Kullanici giris yapti.");
                    return Redirect(Url.Action("GenerateExam", "Exam"));
                    //return Redirect(Url.Action("Index", "Home"));
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Basarısız giriş denemesi.");
                    return View(loginVM);
                }
            }

            return View(loginVM);
        }
        #endregion

        #region LogInUserInfos
        public async Task<UserClaimVM> GetUserInfos()
        {
            var user = HttpContext.User;
            var dbUser = await _userManager.GetUserAsync(user);

            UserClaimVM userClaimViewModel = new UserClaimVM()
            {
                UserId = dbUser.Id,
                UserName = dbUser.UserName,
                Email = dbUser.Email
            };

            return userClaimViewModel;
        }
        #endregion

        #region Logout

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            _logger.LogInformation("Kullanici cikis yapti.");
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        #endregion

        #region Helpers
        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
        }
        #endregion

    }
}