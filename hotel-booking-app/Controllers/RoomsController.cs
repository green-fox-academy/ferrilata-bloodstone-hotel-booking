using HotelBookingApp.Models.HotelModels;
using HotelBookingApp.Pages;
using HotelBookingApp.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace HotelBookingApp.Controllers
{
    [Route("hotel/{hotelId}/rooms")]
    public class RoomsController : Controller
    {
        private readonly IRoomService roomService;

        public RoomsController(IRoomService roomService)
        {
            this.roomService = roomService;
        }

        [HttpGet("{roomId}/reservation/new")]
        public IActionResult NewReservation(int hotelId, int roomId)
        {
            return View(new ReservationViewModel
            {
                HotelId = hotelId,
                RoomId = roomId,
                Reservation = new Reservation()
            });
        }

        [HttpPost("{roomId}/reservation/new")]
        public IActionResult NewReservation(int hotelId, int roomId, ReservationViewModel model)
        {
            return RedirectToAction(nameof(ConfirmReservation), new { hotelId, roomId, model });
        }

        [HttpGet("{roomId}/reservation/confirmation")]
        public IActionResult ConfirmReservation(int hotelId, int roomId, ReservationViewModel model)
        {
            return View(model);
        }
    }
}