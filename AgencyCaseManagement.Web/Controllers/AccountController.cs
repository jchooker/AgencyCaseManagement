using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using AgencyCaseManagement.Domain.Entities;
using AgencyCaseManagement.Web.ViewModels;
using AgencyCaseManagement.Application.DTOs;
using AgencyCaseManagement.Application.Services;

namespace AgencyCaseManagement.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IAccountService _accountService;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager,
            IAccountService accountService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _accountService = accountService;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl = null)
        {
            if (!ModelState.IsValid) return View(model);

            var dto = new LoginDTO
            {
                EmailOrUserName = model.EmailOrUserName,
                Password = model.Password,
                RememberMe = model.RememberMe
            };

            var result = await _accountService.LoginAsync(dto);

            if (result.Succeeded)
            {
                //early dev: sign them in immediately after registration
                return RedirectToAction("Login");
                //redirect to email confirmation pg later?
            }

            foreach (var err in result.Errors)
                ModelState.AddModelError("", err);

            return View(model);


            //var user = await _userManager.FindByEmailAsync(model.Email);

            //if (user == null)
            //{
            //    ModelState.AddModelError("", "Invalid login attempt.");
            //    return View(model);
            //}

            //if (string.IsNullOrEmpty(user.PasswordHash))
            //{
            //    ModelState.AddModelError("", "Your account has not been finalized!");
            //    //^^NEED TO "FINALIZE" WHAT this is going to do!
            //    return View(model);
            //}

            //var validPw = await _userManager.CheckPasswordAsync(user, model.Password);

            //if (!validPw)
            //{
            //    ModelState.AddModelError("", "Invalid login attempt.");
            //    return View(model);
            //}

            //await _signInManager.SignInAsync(user, model.RememberMe);

            //if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl)) return Redirect(returnUrl);

            //return RedirectToAction("Index", "Dashboard");

        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> RegisterAsync(RegisterViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var user = new User
            {
                Id = Guid.NewGuid(),
                Email = model.Email,
                UserName = model.UserName,
                EmailConfirmed = false //if you want to confirm later
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded) 
            { 
                await _signInManager.SignInAsync(user, isPersistent: false);
                return RedirectToAction("Index", "Dashboard");
            }

            foreach (var error in result.Errors)
                ModelState.AddModelError("", error.Description);

            return View(model);

        }
    }
}
