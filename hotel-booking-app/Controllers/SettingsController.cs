using Microsoft.AspNetCore.Mvc;

namespace HotelBookingApp.Controllers
{
    public class SettingsController : Controller
    {

        [HttpGet("Settings")]
        public IActionResult Settings()
        {
            return View();
        }
    }
}
