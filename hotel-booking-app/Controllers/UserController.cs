using System;
using HotelBookingApp.Models.User;
using HotelBookingApp.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using HotelBookingApp.Models;

namespace HotelBookingApp.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService userService;

        public UserController(IUserService userService)
        {
            this.userService = userService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserLoginReq userReq)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Login", "Home", ModelState);
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpPost("signup")]
        public async Task<IActionResult> Signup(UserSignupReq userReq)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Signup", "Home", ModelState);
            }
            userService.Create(new UserModel() { Email = userReq.Email, Password = userReq.Password });
            return RedirectToAction("Index", "Home");
        }
    }
}