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
        private readonly IRoomService roomService;

        public ReservationsController(IReservationService reservationService, IRoomService roomService)
        {
            this.reservationService = reservationService;
            this.roomService = roomService;
        }

        [Authorize(Roles = "Admin, HotelManager")]
        [HttpGet("reservations")]
        public async Task<IActionResult> Index(ReservationViewModel model)
        {
            model.Reservations = await reservationService.FindAllByHotelIdAsync(model.HotelId);
            return View(model);
        }

        [Authorize(Roles = "Admin, User")]
        [HttpGet("/reservations/my-reservations")]
        public async Task<IActionResult> MyReservations()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return View(nameof(Index), new ReservationViewModel
            {
                Reservations = await reservationService.FindAllByUserId(userId)
            });
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
            var reservation = await reservationService.FindByIdAsync(reservationId);
            if (await reservationService.IsIntervalOccupied(reservation))
            {
                return RedirectToAction(nameof(Edit), new { hotelId, roomId = reservation.RoomId, reservationId });
            }
            await reservationService.ConfirmAsync(reservationId);
            return RedirectToAction(nameof(HotelsController.Hotel), "Hotels", new { id = hotelId });
        }

        [Authorize(Roles = "Admin, User")]
        [HttpPost("/reservations/delete/{reservationId}")]
        public async Task<IActionResult> Delete(int reservationId)
        {
            await reservationService.DeleteAsync(reservationId);
            return RedirectToAction(nameof(MyReservations));
        }

        [HttpGet("verifyGuestNumber")]
        public async Task<IActionResult> VerifyGuestNumber(Reservation reservation)
        {
            var room = await roomService.FindByIdAsync(reservation.RoomId);
            if (room.Capacity < reservation.GuestNumber)
            {
                return Json($"This room only has capacity for {room.Capacity} person(s).");
            }
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
        public async Task<IActionResult> VerifyFromDate(Reservation reservation)
        {
            if (await reservationService.IsIntervalOccupied(reservation))
            {
                return Json($"Room is already occupied in this interval: {reservation.FromDate} - {reservation.ToDate}");
            }
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