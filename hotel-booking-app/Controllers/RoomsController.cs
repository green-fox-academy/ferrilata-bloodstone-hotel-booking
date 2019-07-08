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
        private readonly IReservationService roomService;

        public RoomsController(IReservationService roomService)
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
        public async Task<IActionResult> NewReservationAsync(ReservationViewModel model)
        {
            model.Reservation.RoomId = model.RoomId;
            var reservation = await roomService.AddAsync(model.Reservation);
            return RedirectToAction(nameof(ConfirmReservation), new { reservationId = reservation.ReservationId });
        }

        [HttpGet("{roomId}/reservation/confirmation/{reservationId}")]
        public IActionResult ConfirmReservation(int reservationId)
        {
            return View();
        }
    }
}