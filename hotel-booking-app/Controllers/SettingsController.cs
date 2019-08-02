using HotelBookingApp.Models.Account;
using HotelBookingApp.Pages;
using HotelBookingApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace HotelBookingApp.Controllers
{
    [Authorize]
    [Route("[controller]")]
    public class SettingsController : Controller
    {
        private readonly IAccountService accountService;

        public SettingsController(IAccountService accountService)
        {
            this.accountService = accountService;
        }

        [HttpGet]
        public IActionResult Settings()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SetLanguage(string culture)
        {
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
            );

            return RedirectToAction(nameof(Settings));
        }

        [HttpGet("Password")]
        public IActionResult Password()
        {
            return View(new SettingViewModel { });
        }

        [HttpPost("Password")]
        public async Task<IActionResult> Password(SettingViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            model.ApplicationUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            model.ApplicationUser = await accountService.FindByIdAsync(model.ApplicationUserId);

            var errors = await accountService.ChangePasswordAsync(model);
            if (errors.Count == 0)
            {
                return RedirectToAction(nameof(Settings));
            }
            model.ErrorMessages = errors;
            return View(model);
        }
    }
}
