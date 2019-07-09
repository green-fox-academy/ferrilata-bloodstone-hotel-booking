using HotelBookingApp.Models.Account;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace HotelBookingApp.Controllers
{
    public class SettingsController : Controller
    {
        private readonly UserManager<ApplicationUser> UserManager;

        public SettingsController(UserManager<ApplicationUser> userManager)
        {
            this.UserManager = userManager;
        }

        [HttpGet("Settings")]
        public IActionResult Settings()
        {
            return View();
        }

        [HttpPost("Settings")]
        public IActionResult SetLanguage(string culture)
        {
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
            );
            return RedirectToAction(nameof(Settings));
        }
    }
}