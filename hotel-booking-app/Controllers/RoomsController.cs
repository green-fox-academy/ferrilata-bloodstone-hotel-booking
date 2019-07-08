using HotelBookingApp.Models.HotelModels;
using HotelBookingApp.Pages;
using HotelBookingApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace HotelBookingApp.Controllers
{
    [Authorize(Roles = "Admin, HotelManager")]
    [Route("hotel/{hotelId}/rooms")]
    public class RoomsController : Controller
    {
        private readonly IReservationService roomService;
        private readonly IHotelService hotelService;

        public RoomsController(IReservationService roomService, IHotelService hotelService)
        {
            this.roomService = roomService;
            this.hotelService = hotelService;
        }

        [HttpGet("/new")]
        public IActionResult Add()
        {
            return View(new Room());
        }

        [HttpPost("/new")]
        public async Task<IActionResult> Add(int id, Room room)
        {
            await hotelService.AddRoom(id, room);
            return RedirectToAction(nameof(HotelsController.Hotel), "Hotels");
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