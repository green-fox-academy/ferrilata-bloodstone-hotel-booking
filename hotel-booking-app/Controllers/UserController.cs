using HotelBookingApp.Exceptions;
using HotelBookingApp.Models.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace HotelBookingApp.Controllers
{
    public class UserController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;

        public UserController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        [HttpGet("login")]
        public IActionResult Login()
        {
            return View(new UserLoginReq());
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserLoginReq userReq)
        {
            if (!ModelState.IsValid)
            {
                return View(userReq);
            }
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        [HttpGet("signup")]
        public IActionResult Signup()
        {
            return View(new UserSignupReq());
        }

        [HttpPost("signup")]
        public async Task<IActionResult> Signup(UserSignupReq userReq)
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
                userReq.ErrorMessage += err;
            }
            return View(userReq);
        }
    }
}