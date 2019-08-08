using AutoMapper;
using HotelBookingApp.Exceptions;
using HotelBookingApp.Models.Account;
using HotelBookingApp.Models.API;
using HotelBookingApp.Models.HotelModels;
using HotelBookingApp.Pages;
using HotelBookingApp.Services;
using HotelBookingApp.Utils;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace HotelBookingApp.Controllers
{
    [Produces("application/json")]
    [Route("[controller]")]
    [ApiController]
    public class ApiController : ControllerBase
    {
        private readonly IHotelService hotelService;
        private readonly IRoomService roomService;
        private readonly IAccountService accountService;
        private readonly IReservationService reservationService;
        private readonly IMapper mapper;
        private const string authScheme = JwtBearerDefaults.AuthenticationScheme;


        public ApiController(IHotelService hotelService, IRoomService roomService, IAccountService accountService, IReservationService reservationService, IMapper mapper)
        {
            this.hotelService = hotelService;
            this.roomService = roomService;
            this.accountService = accountService;
            this.reservationService = reservationService;
            this.mapper = mapper;
        }

        /// <summary>
        /// After successful login attempt returns a token.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /api/login
        ///     {
        ///     "Email": "user1@bloodstone.com",
        ///     "Password": "Password66", 
        ///     "RememberMe": "false"
        ///     }
        ///
        /// </remarks>
        /// <param name="request"></param>
        /// <returns>Returns a LoginResponseDTO object. If successful, token will be filled, otherwise see errors.</returns>
        /// <response code="200">Returns LoginResponseDTO with token. (LoginResponseDTO.token)</response>
        /// <response code="400">If login attempt is unsuccessful, returns with LoginResponseDTO error list. (LoginResponseDTO.error)</response>    
        [ProducesResponseType(typeof(LoginResponseDTO), 200)]
        [ProducesResponseType(typeof(List<string>), 400)]
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
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType(typeof(List<string>), 400)]
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

        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
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
        [HttpPut("hotels/{hotelId}/rooms/{roomId}/reserve")]
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
        [HttpPut("user/reservations/{reservationId}/confirm")]
        public async Task<IActionResult> Confirm(int reservationId, [FromBody] Reservation reservation)
        {
            var reservationIntervalOccupied = await reservationService.IsIntervalOccupied(reservation);
            if (reservationIntervalOccupied)
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
        [HttpDelete("user/reservations/{reservationId}/delete")]
        public async Task<IActionResult> Delete(int reservationId)
        {
            await reservationService.DeleteAsync(reservationId);
            return NoContent();
        }

        [Authorize(AuthenticationSchemes = authScheme, Roles = "User")]
        [HttpGet("user/reservations")]
        public async Task<IActionResult> FindAllReservations()
        {
            var reservations = await reservationService.FindAllByUserId(GetUserId());
            if (reservations != null && reservations.ToList().Count > 0)
            {
                var reservationList = reservations.ToList();
                var dtoList = mapper.Map<List<ReservationDTO>>(reservationList);
                return Ok(dtoList);
            }
            return NoContent();
        }

        [Authorize(AuthenticationSchemes = authScheme, Roles = "User")]
        [HttpGet("user/reservations/cleanup")]
        public async Task<IActionResult> CleanUp()
        {
            await reservationService.CleanUp(GetUserId());
            return Ok("Old reservation have been removed.");
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

        [Authorize(AuthenticationSchemes = authScheme, Roles = "User")]
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

        [Authorize(AuthenticationSchemes = authScheme, Roles = "User")]
        [HttpPost("hotels/{hotelId}/addReview")]
        public async Task<IActionResult> AddReview(int hotelId, [FromBody] Review review)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(review);
            }
            review.HotelId = hotelId;
            review.ApplicationUserId = GetUserId();
            await hotelService.AddReviewAsync(review);
            return Ok("Review added!");
        }

        [Authorize(AuthenticationSchemes = authScheme, Roles = "User")]
        [HttpDelete("hotels/{hotelId}/review/{reviewId}/delete")]
        public async Task<IActionResult> AddReview(int hotelId, int reviewId)
        {
            try
            {
                await hotelService.DeleteReview(reviewId);
            } catch (ItemNotFoundException e)
            {
                return BadRequest(e.Message);
            }
            return NoContent();
        }

        private string GetUserId()
        {
            return User.FindFirst(ClaimTypes.NameIdentifier).Value;
        }
    }
}