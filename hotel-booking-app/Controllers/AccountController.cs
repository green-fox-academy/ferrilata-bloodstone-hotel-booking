using HotelBookingApp.Exceptions;
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
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;

        public AccountController(IAccountService accountService, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            this.accountService = accountService;
            this.userManager = userManager;
            this.signInManager = signInManager;
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
            var errors = await accountService.SignIn(request);
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
            await accountService.SignOut();
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
            var user = new ApplicationUser { UserName = request.Email, Email = request.Email };
            var result = await userManager.CreateAsync(user, request.Password);
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(user, "User");
                await signInManager.SignInAsync(user, isPersistent: false);
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
            foreach (var err in result.Errors)
            {
                request.ErrorMessage += err.Description;
            }
            return View(request);
        }

        [HttpGet("access-denied")]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}