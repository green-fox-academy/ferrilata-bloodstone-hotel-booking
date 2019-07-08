using Microsoft.AspNetCore.Mvc;

namespace HotelBookingApp.Controllers
{
    [Route("hotel/{hotelId}/rooms")]
    public class RoomsController : Controller
    {
        [HttpGet("{roomId}/reservation/new")]
        public IActionResult NewReservation(int hotelId, int roomId)
        {
            return View();
        }
    }
}