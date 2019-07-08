using HotelBookingApp.Pages;
using Microsoft.AspNetCore.Mvc;

namespace HotelBookingApp.Controllers
{
    [Route("hotel/{hotelId}/rooms")]
    public class RoomsController : Controller
    {
        [HttpGet("{roomId}/reservation/new")]
        public IActionResult NewReservation(int hotelId, int roomId)
        {
            return View(new RoomViewModel {
                HotelId = hotelId
            });
        }

        [HttpPost("{roomId}/reservation/new")]
        public IActionResult NewReservation(int hotelId, int roomId, RoomViewModel model)
        {
            return RedirectToAction(nameof(ConfirmReservation), new { hotelId, roomId, model });
        }

        [HttpGet("{roomId}/reservation/confirmation")]
        public IActionResult ConfirmReservation(int hotelId, int roomId, RoomViewModel model)
        {
            return View(model);
        }
    }
}