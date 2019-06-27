using System;
using HotelBookingApp.Models.User;
using HotelBookingApp.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using HotelBookingApp.Exceptions;

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
            try
            {
                await userService.Create(new UserModel() { Email = userReq.Email, Password = userReq.Password });
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
            catch (ResourceAlreadyExistsException ex)
            {
                userReq.ErrorMessage = ex.Message;
                return View(userReq);
            }
        }
    }
}