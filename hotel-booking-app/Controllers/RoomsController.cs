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
        private readonly IReservationService reservationService;
        private readonly IHotelService hotelService;

        public RoomsController(IReservationService reservationService, IHotelService hotelService)
        {
            this.reservationService = reservationService;
            this.hotelService = hotelService;
        }

        [HttpGet("new")]
        public IActionResult Add()
        {
            return View(new Room());
        }

        [HttpPost("new")]
        public async Task<IActionResult> Add(int hotelId, Room room)
        {
            await hotelService.AddRoom(hotelId, room);
            return RedirectToAction(nameof(HotelsController.Hotel), "Hotels", new { id = hotelId });
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
        public async Task<IActionResult> NewReservation(ReservationViewModel model)
        {
            model.Reservation.RoomId = model.RoomId;
            var reservation = await reservationService.AddAsync(model.Reservation);
            return RedirectToAction(nameof(ConfirmReservation), new { reservationId = reservation.ReservationId });
        }

        [HttpGet("{roomId}/reservation/confirmation/{reservationId}")]
        public async Task<IActionResult> ConfirmReservation(int reservationId)
        {
            return View(new ReservationViewModel
            {
                Reservation = await reservationService.FindById(reservationId)
            });
        }

        [HttpPost("{roomId}/reservation/confirmation/{reservationId}")]
        public async Task<IActionResult> ConfirmReservation(int hotelId, int reservationId)
        {
            await reservationService.Confirm(reservationId);
            return RedirectToAction(nameof(HotelsController.Hotel), "Hotels", new { id = hotelId });
        }
    }
}