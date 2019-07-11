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
        public IActionResult Add(int hotelId, int roomId)
        {
            return View(new ReservationViewModel { HotelId = hotelId, RoomId = roomId });
        }

        [Authorize(Roles = "User, Admin")]
        [HttpPost("rooms/{roomId}/reservations/new")]
        public async Task<IActionResult> Add(ReservationViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            model.Reservation.ApplicationUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var reservation = await reservationService.AddAsync(model.Reservation);
            return RedirectToAction(nameof(Confirm), new { reservationId = reservation.ReservationId });
        }

        [Authorize(Roles = "User, Admin")]
        [HttpGet("rooms/{roomId}/reservations/edit/{reservationId}")]
        public async Task<IActionResult> Edit(int hotelId, int roomId, int reservationId)
        {
            var reservation = await reservationService.FindByIdAsync(reservationId);
            return View(nameof(Add), new ReservationViewModel
            {
                HotelId = hotelId, RoomId = roomId, Reservation = reservation
            });
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

        [HttpGet("verifyGuestNumber")]
        public IActionResult VerifyGuestNumber(Reservation reservation)
        {
            return Json(true);
        }

        [HttpGet("verifyGuestNames")]
        public IActionResult VerifyGuestNames(Reservation reservation)
        {
            if (reservation.GuestNames.Split(",").Length != reservation.GuestNumber)
            {
                return Json("Number of guest names does not match the number of guests.");
            }
            return Json(true);
        }

        [HttpGet("verifyFromDate")]
        public IActionResult VerifyFromDate(Reservation reservation)
        {
            return Json(true);
        }

        [HttpGet("verifyToDate")]
        public IActionResult VerifyToDate(Reservation reservation)
        {
            if (reservation.FromDate >= reservation.ToDate)
            {
                return Json("To date must be later.");
            }
            return Json(true);
        }
    }
}