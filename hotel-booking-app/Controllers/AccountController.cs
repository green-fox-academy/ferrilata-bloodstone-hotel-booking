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

        [HttpPost("logout")]
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
    }
}