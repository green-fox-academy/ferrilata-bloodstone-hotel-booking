using AutoMapper;
using HotelBookingApp.Exceptions;
using HotelBookingApp.Models.User;
using HotelBookingApp.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace HotelBookingApp.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService userService;
        private readonly IMapper mapper;

        public UserController(IUserService userService, IMapper mapper)
        {
            this.userService = userService;
            this.mapper = mapper;
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
                await userService.Create(mapper.Map<UserSignupReq, UserModel>(userReq));
                return RedirectToAction(nameof(Login));
            }
            catch (ResourceAlreadyExistsException ex)
            {
                userReq.ErrorMessage = ex.Message;
                return View(userReq);
            }
        }
    }
}