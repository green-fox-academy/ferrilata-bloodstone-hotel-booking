using HotelBookingApp.Models.HotelModels;
using HotelBookingApp.Pages;
using HotelBookingApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace HotelBookingApp.Controllers
{
    [Authorize(Roles = "Admin, HotelManager")]
    [Route("hotel/{hotelId}")]
    public class RoomsController : Controller
    {
        private readonly IReservationService reservationService;
        private readonly IHotelService hotelService;

        public RoomsController(IReservationService reservationService, IHotelService hotelService)
        {
            this.reservationService = reservationService;
            this.hotelService = hotelService;
        }

        [HttpGet("rooms/new")]
        public IActionResult Add()
        {
            return View(new Room());
        }

        [HttpPost("rooms/new")]
        public async Task<IActionResult> Add(int hotelId, Room room)
        {
            await hotelService.AddRoom(hotelId, room);
            return RedirectToAction(nameof(HotelsController.Hotel), "Hotels", new { id = hotelId });
        }

        [HttpGet("rooms/{roomId}/reservations/new")]
        public IActionResult NewReservation(int hotelId, int roomId)
        {
            return View(new ReservationViewModel
            {
                HotelId = hotelId,
                RoomId = roomId,
                Reservation = new Reservation()
            });
        }

        [HttpPost("rooms/{roomId}/reservations/new")]
        public async Task<IActionResult> NewReservation(ReservationViewModel model)
        {
            model.Reservation.RoomId = model.RoomId;
            var reservation = await reservationService.AddAsync(model.Reservation);
            return RedirectToAction(nameof(ConfirmReservation), new { reservationId = reservation.ReservationId });
        }

        [HttpGet("rooms/{roomId}/reservations/confirmation/{reservationId}")]
        public async Task<IActionResult> ConfirmReservation(int reservationId)
        {
            return View(new ReservationViewModel
            {
                Reservation = await reservationService.FindByIdAsync(reservationId)
            });
        }

        [HttpPost("rooms/{roomId}/reservations/confirmation/{reservationId}")]
        public async Task<IActionResult> ConfirmReservation(int hotelId, int reservationId)
        {
            await reservationService.ConfirmAsync(reservationId);
            return RedirectToAction(nameof(HotelsController.Hotel), "Hotels", new { id = hotelId });
        }

        [HttpGet("reservations")]
        public async Task<IActionResult> GetReservations(int hotelId)
        {
            var reservations = await reservationService.FindAllByHotelIdAsync(hotelId);
            return View(reservations);
        }
    }
}