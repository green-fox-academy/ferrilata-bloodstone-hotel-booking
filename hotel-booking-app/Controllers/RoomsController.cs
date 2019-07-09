using HotelBookingApp.Models.HotelModels;
using HotelBookingApp.Pages;
using HotelBookingApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace HotelBookingApp.Controllers
{
    [Authorize]
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

        [Authorize(Roles = "Admin, HotelManager")]
        [HttpGet("rooms/new")]
        public IActionResult Add()
        {
            return View(new Room());
        }

        [Authorize(Roles = "Admin, HotelManager")]
        [HttpPost("rooms/new")]
        public async Task<IActionResult> Add(int hotelId, Room room)
        {
            await hotelService.AddRoom(hotelId, room);
            return RedirectToAction(nameof(HotelsController.Hotel), "Hotels", new { id = hotelId });
        }

        [Authorize(Roles = "User, Admin")]
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

        [Authorize(Roles = "User, Admin")]
        [HttpPost("rooms/{roomId}/reservations/new")]
        public async Task<IActionResult> NewReservation(ReservationViewModel model)
        {
            model.Reservation.ApplicationUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var reservation = await reservationService.AddAsync(model.Reservation);
            return RedirectToAction(nameof(ConfirmReservation), new { reservationId = reservation.ReservationId });
        }

        [Authorize(Roles = "User, Admin")]
        [HttpGet("rooms/{roomId}/reservations/edit/{reservationId}")]
        public async Task<IActionResult> EditReservation(ReservationViewModel model)
        {
            model.Reservation = await reservationService.FindByIdAsync(model.ReservationId);
            return View(model);
        }

        [Authorize(Roles = "User, Admin")]
        [HttpGet("rooms/{roomId}/reservations/confirmation/{reservationId}")]
        public async Task<IActionResult> ConfirmReservation(ReservationViewModel model)
        {
            model.Reservation = await reservationService.FindByIdAsync(model.ReservationId);
            return View(model);
        }

        [Authorize(Roles = "User, Admin")]
        [HttpPost("rooms/{roomId}/reservations/confirmation/{reservationId}")]
        public async Task<IActionResult> ConfirmReservation(int hotelId, int reservationId)
        {
            await reservationService.ConfirmAsync(reservationId);
            return RedirectToAction(nameof(HotelsController.Hotel), "Hotels", new { id = hotelId });
        }

        [Authorize(Roles = "Admin, HotelManager")]
        [HttpGet("reservations")]
        public async Task<IActionResult> GetReservations(int hotelId)
        {
            var reservations = await reservationService.FindAllByHotelIdAsync(hotelId);
            return View(reservations);
        }

        [Authorize(Roles = "Admin, HotelManager")]
        [HttpPost("reservation/delete/{reservationId}")]
        public async Task<IActionResult> DeleteReservation(int hotelId, int reservationId)
        {
            await reservationService.DeleteAsync(reservationId);
            return RedirectToAction(nameof(GetReservations), new { id = hotelId });
        }
    }
}