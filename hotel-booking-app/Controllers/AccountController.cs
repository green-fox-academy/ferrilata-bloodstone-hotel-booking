using HotelBookingApp.Models.Account;
using HotelBookingApp.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace HotelBookingApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountService accountService;

        public AccountController(IAccountService accountService)
        {
            this.accountService = accountService;
        }

        [HttpGet("login")]
        public async Task<IActionResult> Login()
        {
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);
            return View(new LoginRequest());
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest request, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(request);
            }
            var errors = await accountService.SignInAsync(request);
            if (errors.Count == 0)
            {
                returnUrl = returnUrl ?? Url.Content("~/");
                return LocalRedirect(returnUrl);
            }
            request.ErrorMessages = errors;
            return View(request);
        }

        [HttpGet("/Google-login")]
        public IActionResult GoogleLogin()
        {
            string redirectUrl = "Google-response";
            var properties = accountService.ConfigureExternalAuthenticationProperties("Google", redirectUrl);
            return new ChallengeResult("Google", properties);
        }

        [HttpGet("Google-response")]
        public async Task<IActionResult> GoogleResponse()
        {
            var info = await accountService.GetExternalLoginInfoAsync();
            if (info == null)
            {
                return RedirectToAction(nameof(Login));
            }
            var result = await accountService.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, false);

            if (!result.Succeeded)
            {
                await accountService.CreateAndLoginGoogleUser(info);
            }
            return RedirectToAction(nameof(HotelsController.Index), "Hotels");
        }

        [HttpGet("logout")]
        public async Task<IActionResult> Logout()
        {
            await accountService.SignOutAsync();
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        [HttpGet("signup")]
        public IActionResult Signup()
        {
            return View(new SignupRequest());
        }

        [HttpPost("signup")]
        public async Task<IActionResult> Signup(SignupRequest request)
        {
            if (!ModelState.IsValid)
            {
                return View(request);
            }
            var errors = await accountService.SignUpAsync(request);
            if (errors.Count == 0)
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
            request.ErrorMessages = errors;
            return View(request);
        }

        [HttpGet("access-denied")]
        public IActionResult AccessDenied()
        {
            return View();
        }

        [HttpGet("Reset-Password")]
        public IActionResult ResetPassword()
        {
            return View();
        }

        [HttpPost("Reset-Password")]
        public async Task<IActionResult> ResetPasswordRequest(string email)
        {
            var password = accountService.CreateRandomPassword();
            await accountService.ResetPasswordAsync(email, password);
            return View();
        }
    }
}