using HotelBookingApp.Exceptions;
using HotelBookingApp.Models.Account;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace HotelBookingApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        [HttpGet("login")]
        public IActionResult Login()
        {
            return View(new LoginRequest());
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest userReq)
        {
            if (!ModelState.IsValid)
            {
                return View(userReq);
            }
            var result = await signInManager.PasswordSignInAsync(userReq.Email, userReq.Password, true, true);
            if (result.Succeeded)
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
            if (result.IsLockedOut)
            {
                userReq.ErrorMessage = "User account locked out.";
            }
            userReq.ErrorMessage = "Invalid login attempt.";
            return View(userReq);
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        [HttpGet("signup")]
        public IActionResult Signup()
        {
            return View(new SignupRequest());
        }

        [HttpPost("signup")]
        public async Task<IActionResult> Signup(SignupRequest userReq)
        {
            if (!ModelState.IsValid)
            {
                return View(userReq);
            }
            var user = new ApplicationUser { UserName = userReq.Email, Email = userReq.Email };
            var result = await userManager.CreateAsync(user, userReq.Password);
            if (result.Succeeded)
            {
                await signInManager.SignInAsync(user, isPersistent: false);
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
            foreach (var err in result.Errors)
            {
                userReq.ErrorMessage += err.Description;
            }
            return View(userReq);
        }
    }
}