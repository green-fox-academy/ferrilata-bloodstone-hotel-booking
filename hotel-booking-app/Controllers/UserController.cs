using System;
using HotelBookingApp.Models.User;
using HotelBookingApp.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace HotelBookingApp.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService userService;

        public UserController(IUserService userService)
        {
            this.userService = userService;
        }

        [HttpGet("login")]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserLoginReq userReq)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Login", ModelState);
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpGet("signup")]
        public IActionResult Signup()
        {
            return View();
        }

        [HttpPost("signup")]
        public async Task<IActionResult> Signup(UserSignupReq userReq)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Signup", ModelState);
            }
            await userService.Create(new UserModel() { Email = userReq.Email, Password = userReq.Password });    
            return RedirectToAction("Index", "Home");
        }
    }
}