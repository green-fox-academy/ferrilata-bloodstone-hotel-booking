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
    [Route("hotels/{hotelId}")]
    public class ReservationsController : Controller
    {
        private readonly IReservationService reservationService;

        public ReservationsController(IReservationService reservationService)
        {
            this.reservationService = reservationService;
        }

        [Authorize(Roles = "Admin, HotelManager")]
        [HttpGet("reservations")]
        public async Task<IActionResult> Index(ReservationViewModel model)
        {
            model.Reservations = await reservationService.FindAllByHotelIdAsync(model.HotelId);
            return View(model);
        }

        [Authorize(Roles = "User, Admin")]
        [HttpGet("rooms/{roomId}/reservations/new")]
        public IActionResult Add(ReservationViewModel model)
        {
            return View(model);
        }

        [Authorize(Roles = "User, Admin")]
        [HttpPost("rooms/{roomId}/reservations/new")]
        public async Task<IActionResult> Add(Reservation reservation)
        {
            reservation.ApplicationUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var newReservation = await reservationService.AddAsync(reservation);
            return RedirectToAction(nameof(Confirm), new { reservationId = newReservation.ReservationId });
        }

        [Authorize(Roles = "User, Admin")]
        [HttpGet("rooms/{roomId}/reservations/edit/{reservationId}")]
        public async Task<IActionResult> Edit(ReservationViewModel model)
        {
            model.Reservation = await reservationService.FindByIdAsync(model.ReservationId);
            return View(nameof(Add), model);
        }

        [Authorize(Roles = "User, Admin")]
        [HttpGet("rooms/{roomId}/reservations/confirmation/{reservationId}")]
        public async Task<IActionResult> Confirm(ReservationViewModel model)
        {
            model.Reservation = await reservationService.FindByIdAsync(model.ReservationId);
            return View(model);
        }

        [Authorize(Roles = "User, Admin")]
        [HttpPost("rooms/{roomId}/reservations/confirmation/{reservationId}")]
        public async Task<IActionResult> Confirm(int hotelId, int reservationId)
        {
            await reservationService.ConfirmAsync(reservationId);
            return RedirectToAction(nameof(HotelsController.Hotel), "Hotels", new { id = hotelId });
        }

        [Authorize(Roles = "Admin, HotelManager")]
        [HttpPost("reservations/delete/{reservationId}")]
        public async Task<IActionResult> Delete(int hotelId, int reservationId)
        {
            await reservationService.DeleteAsync(reservationId);
            return RedirectToAction(nameof(Index), new { id = hotelId });
        }
    }
}