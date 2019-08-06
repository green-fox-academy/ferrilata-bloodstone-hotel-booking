using HotelBookingApp.Exceptions;
using HotelBookingApp.Models.Account;
using HotelBookingApp.Models.HotelModels;
using HotelBookingApp.Pages;
using HotelBookingApp.Services;
using HotelBookingApp.Utils;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace HotelBookingApp.Controllers
{
    [Route("[controller]/")]
    [ApiController]
    public class ApiController : ControllerBase
    {
        private readonly IHotelService hotelService;
        private readonly IRoomService roomService;
        private readonly IAccountService accountService;
        private readonly IReservationService reservationService;
        private const string authScheme = JwtBearerDefaults.AuthenticationScheme;


        public ApiController(IHotelService hotelService, IRoomService roomService, IAccountService accountService, IReservationService reservationService)
        {
            this.hotelService = hotelService;
            this.roomService = roomService;
            this.accountService = accountService;
            this.reservationService = reservationService;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var response = await accountService.SignInApiAsync(request);
            if (response.token == null)
            {
                return BadRequest(string.Join(", ", response.errors.ToArray()));
            }
            return Ok(response);
        }
        
        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] SignupRequest request)
        {
            var errors = await accountService.SignUpAsync(request);
            if (errors.Count != 0)
            {
                return BadRequest(string.Join(", ", errors.ToArray()));
            }
            return Ok("Registration Successful!");
        }

        [AllowAnonymous]
        [HttpGet("hotels")]
        public async Task<IActionResult> Hotels(string city, int currentPage = 1)
        {
            var paginatedHotels = await hotelService.FindWithQuery(new QueryParams
            {
                CurrentPage = currentPage,
                Search = city
            });
            if (paginatedHotels.TotalPages < paginatedHotels.CurrentPage)
            {
                return BadRequest(string.Format("Error: the current page is greater than the number of pages. Max. page: {0}", paginatedHotels.TotalPages));
            }
            return Ok(hotelService.GetHotelDTOs(paginatedHotels));
        }


        [Authorize(AuthenticationSchemes = authScheme, Roles = "User")]
        [HttpGet("hotels/{hotelId}/rooms")]
        public async Task<IActionResult> Rooms(int hotelId)
        {
            try
            {
                return Ok(await roomService.GetRoomDTOs(hotelId));
            }
            catch (ItemNotFoundException e)
            {
                return BadRequest(e.Message);
            }
        }

        [Authorize(AuthenticationSchemes = authScheme, Roles = "User")]
        [HttpPost("hotels/{hotelId}/rooms/{roomId}/reserve")]
        public async Task<IActionResult> Reserve(int roomId, [FromBody] ReservationViewModel model)
        {
            model.Reservation.ApplicationUserId = GetUserId();
            model.Reservation.RoomId = roomId;
            if (!ModelState.IsValid)
            {
                return BadRequest(model);
            }
            var reservation = await reservationService.AddAsync(model.Reservation);
            return Ok(reservation);
        }

        
        [Authorize(AuthenticationSchemes = authScheme, Roles = "User")]
        [HttpPost("reservation/confirm")]
        public async Task<IActionResult> Confirm([FromBody] Reservation reservation)
        {
            var reservationIntervalOccupied = await reservationService.IsIntervalOccupied(reservation);
            if (reservationIntervalOccupied && reservation.ApplicationUserId == GetUserId())
            {
                if (reservationIntervalOccupied)
                {
                    return BadRequest("Interval is already occupied.");
                }
                return BadRequest("Confirmation is not successful.");
            }
            await reservationService.ConfirmAsync(reservation.ReservationId, User.Identity.Name);
            return Ok(reservation);
        }
        
        [Authorize(AuthenticationSchemes = authScheme, Roles = "User")]
        [HttpDelete("reservation/delete")]
        public async Task<IActionResult> Delete([FromBody] int reservationId)
        {
            await reservationService.DeleteAsync(reservationId);
            return NoContent();
        }
        
        [AllowAnonymous]
        [HttpPost("user/resetPassword")]
        public async Task<IActionResult> ResetPassword([FromBody] PasswordResetRequest passwordResetRequest)
        {
            passwordResetRequest.ErrorMessages = await accountService.ResetPasswordAsync(passwordResetRequest.Email);
            if (passwordResetRequest.ErrorMessages.Count != 0)
            {
                return BadRequest(string.Join(", ", passwordResetRequest.ErrorMessages.ToArray()));
            }
            return Ok("Email sent.");
        }

        [Authorize(AuthenticationSchemes = authScheme, Roles = "User")]
        [HttpGet("user/logout")]
        public async Task<IActionResult> Logout()
        {
            await accountService.SignOutAsync();
            return Ok("Signed out successfully.");
        }

        private string GetUserId()
        {
            return User.FindFirst(ClaimTypes.NameIdentifier).Value;
        }
        [HttpPost("user/changePassword")]
        public async Task<IActionResult> ChangePassword(SettingViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(model.ErrorMessages);
            }
            model.ApplicationUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            model.ApplicationUser = await accountService.FindByIdAsync(model.ApplicationUserId);
            var errors = await accountService.ChangePasswordAsync(model);
            if (errors.Count == 0)
            {
                return Ok("Password changed successfully.");
            }
            model.ErrorMessages = errors;
            return BadRequest(model.ErrorMessages);
        }
    }
}