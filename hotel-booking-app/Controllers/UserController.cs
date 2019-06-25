using HotelBookingApp.Models.User;
using HotelBookingApp.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

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
            return View();
        }
    }
}