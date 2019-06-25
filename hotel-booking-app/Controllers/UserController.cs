using System;
using HotelBookingApp.Models.User;
using HotelBookingApp.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using HotelBookingApp.Models;

namespace HotelBookingApp.Controllers
{
    [Route("[controller]")]
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
            return RedirectToAction();
        }

        [HttpPost("signup")]
        public async Task<IActionResult> Signup(UserSignupReq userReq)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            userService.Create(new UserModel() { Email = userReq.Email, Password = userReq.Password });
            return RedirectToAction();
        }
    }
}