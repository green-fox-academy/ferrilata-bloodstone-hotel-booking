using AutoMapper;
using HotelBookingApp.Exceptions;
using HotelBookingApp.Models.Account;
using HotelBookingApp.Models.API;
using HotelBookingApp.Models.HotelModels;
using HotelBookingApp.Models.Image;
using HotelBookingApp.Pages;
using HotelBookingApp.Services;
using HotelBookingApp.Utils;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
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
        private readonly IImageService imageService;
        private const string authScheme = JwtBearerDefaults.AuthenticationScheme;


        public ApiController(IHotelService hotelService, IRoomService roomService, IAccountService accountService, IReservationService reservationService, IMapper mapper, IImageService imageService)
        {
            this.hotelService = hotelService;
            this.roomService = roomService;
            this.accountService = accountService;
            this.reservationService = reservationService;
            this.mapper = mapper;
            this.imageService = imageService;
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
        /// <response code="400">If login attempt failed, returns with LoginResponseDTO error list. (LoginResponseDTO.error)</response>
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

        /// <summary>
        /// Registration with email and password as a simple User.
        /// Option to register as Hotel Manager is not possible.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /api/register
        ///     {
        ///     "Email": "user1@bloodstone.com",
        ///     "Password": "Password66", 
        ///     "VerifyPassword": "Password66",
        ///     }
        ///
        /// </remarks>
        /// <param name="request"></param>
        /// <returns>Returns a LoginResponseDTO object. If successful, returns with string message and HTTP200, otherwise with HTTP400 and see errors.</returns>
        /// <response code="200">Returns a message "Registration Successful!".</response>
        /// <response code="400">Returns a list of strings of errors.</response>
        [AllowAnonymous]
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType(typeof(List<string>), 400)]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] SignupRequest request)
        {
            request.IsManager = false;
            var errors = await accountService.SignUpAsync(request);
            if (errors.Count != 0)
            {
                return BadRequest(string.Join(", ", errors.ToArray()));
            }
            return Ok("Registration Successful!");
        }

        /// <summary>
        /// Get a list of hotels, but max. 10 at once.
        /// Current page can be changed with "currentPage" parameter.
        /// Hotels can be filtered with given "city" parameter.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /api/hotels
        ///     
        /// </remarks>
        /// <param name="city"></param>
        /// <param name="currentPage"></param>
        /// <returns>Returns a paginated list of hotels with required page. Max. 10 hotels/page.</returns>
        /// <response code="200">Returns a paginated list of hotels.</response>
        /// <response code="400">Returns a string with error message.</response>
        [ProducesResponseType(typeof(PaginatedList<Hotel>), 200)]
        [ProducesResponseType(typeof(string), 400)]
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

        /// <summary>
        /// [Authorized] Fetch a hotel by id.
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     GET /api/hotels/1
        /// 
        /// </remarks>
        /// <param name="hotelId"></param>
        /// <returns>A Hotel object.</returns>
        /// <response code="200">Returns a hotel object.</response>
        /// <response code="400">Returns a string with error message.</response>
        [ProducesResponseType(typeof(Hotel), 200)]
        [ProducesResponseType(typeof(string), 400)]
        [Authorize(AuthenticationSchemes = authScheme, Roles = "User")]
        [HttpGet("hotels/{hotelId}")]
        public async Task<IActionResult> FindHotel(int hotelId)
        {
            try
            {
                var hotel = await hotelService.FindByIdAsync(hotelId, new QueryParams());
                return Ok(mapper.Map<HotelDTO>(hotel));
            }
            catch (ItemNotFoundException e)
            {
                return BadRequest(e.Message);
            }
        }

        /// <summary>
        /// Get all images of a hotel.
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     GET /api/1/images
        /// 
        /// </remarks>
        /// <param name="hotelId"></param>
        /// <returns></returns>
        /// <response code="200">Returns list of imageDetails objects.</response>
        /// <response code="500">Returns internal server error if image storage not available.</response>
        [ProducesResponseType(typeof(List<ImageDetails>), 200)]
        [ProducesResponseType(500)]
        [Authorize(AuthenticationSchemes = authScheme, Roles = "User")]
        [HttpGet("hotels/{hotelId}/images")]
        public async Task<IActionResult> FindAllHotelImages(int hotelId)
        {
            try
            {
                var imageList = await imageService.GetImageListAsync(hotelId);
                return Ok(imageList);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        /// <summary>
        /// [Authorized] Rooms of a hotel can be fetched based on hotel id.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET hotels/2/rooms
        ///     
        /// </remarks>
        /// <param name="hotelId"></param>
        /// <returns>List of RoomDTO.</returns>
        /// <response code="200">Returns a list of RoomDTO.</response>
        /// <response code="400">Returns a string with error message.</response>
        [ProducesResponseType(typeof(List<RoomDTO>), 200)]
        [ProducesResponseType(typeof(string), 400)]
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

        /// <summary>
        /// [Authorized] A room can be reserved based on room id and given DTO (ReservationViewModel).
        /// "isConfirmed" property cannot be updated by this, always set to false.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     PUT hotels/2/rooms/11/reserve
        ///     {
        ///     "Reservation": {
        ///         "GuestNames": "John Doe, John Snow",
        ///         "GuestNumber": "2",
        ///         "FromDate": "2019.09.29 0:00:00",
        ///         "ToDate": "2019.09.30 0:00:00",
        ///         }
        ///     }
        ///     
        /// </remarks>
        /// <param name="roomId"></param>
        /// <param name="model"></param>
        /// <returns>A Reservation object.</returns>
        /// <response code="200">Returns the newely created Reservation object.</response>
        /// <response code="400">Returns the ReservationViewModel validation errors.</response>
        [ProducesResponseType(typeof(Reservation), 200)]
        [ProducesResponseType(400)]
        [Authorize(AuthenticationSchemes = authScheme, Roles = "User")]
        [HttpPut("hotels/{hotelId}/rooms/{roomId}/reserve")]
        public async Task<IActionResult> Reserve(int roomId, [FromBody] ReservationViewModel model)
        {
            model.Reservation.ApplicationUserId = GetUserId();
            model.Reservation.RoomId = roomId;
            model.Reservation.IsConfirmed = false;
            if (!ModelState.IsValid)
            {
                return BadRequest(model);
            }
            var reservation = await reservationService.AddAsync(model.Reservation);
            return Ok(reservation);
        }

        /// <summary>
        /// [Authorized] Reservation can be confirmed if the date interval is not already occupied.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     PUT user/reservations/9050/confirm
        ///     {
        ///         "reservationId": 9050,
        ///         "guestNumber": 2,
        ///         "guestNames": "John Doe, John Snow",
        ///         "fromDate": "2019-09-28T00:00:00",
        ///         "toDate": "2019-09-29T00:00:00",
        ///         "isConfirmed": false,
        ///         "room": null,
        ///         "roomId": 6,
        ///         "applicationUser": null,
        ///         "applicationUserId": "cea5b918-252d-47db-aa00-b057ffab37d4",
        ///         "numberOfNights": 1,
        ///         "isCancelable": true
        ///     }
        ///     
        /// </remarks>
        /// <param name="reservationId"></param>
        /// <param name="reservation"></param>
        /// <returns>A confirmed reservation object.</returns>
        /// <response code="200">Returns the updated Reservation object.</response>
        /// <response code="400">Returns an error message: "Interval is already occupied." .</response>
        [ProducesResponseType(typeof(Reservation), 200)]
        [ProducesResponseType(typeof(string), 400)]
        [Authorize(AuthenticationSchemes = authScheme, Roles = "User")]
        [HttpPut("user/reservations/{reservationId}/confirm")]
        public async Task<IActionResult> Confirm(int reservationId, [FromBody] Reservation reservation)
        {
            var reservationIntervalOccupied = await reservationService.IsIntervalOccupied(reservation);

            if (reservationIntervalOccupied)
            {
                return BadRequest("Interval is already occupied.");
            }
            await reservationService.ConfirmAsync(reservation.ReservationId, User.Identity.Name);
            return Ok(reservation);
        }

        /// <summary>
        /// [Authorized] Reservation can be deleted based on reservation id.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     DELETE /api/user/reservations/9050/delete
        ///     
        /// </remarks>
        /// <param name="reservationId"></param>
        /// <returns>Returns No Content status.</returns>
        /// <response code="204">Returns no content.</response>
        [ProducesResponseType(204)]
        [Authorize(AuthenticationSchemes = authScheme, Roles = "User")]
        [HttpDelete("user/reservations/{reservationId}/delete")]
        public async Task<IActionResult> Delete(int reservationId)
        {
            await reservationService.DeleteAsync(reservationId);
            return NoContent();
        }

        /// <summary>
        /// [Authorized] Get all the reservations of the logged in user.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /api/user/reservations
        ///     
        /// </remarks>
        /// <returns>A list of ReservationDTO.</returns>
        /// <response code="200">Returns a list of ReservationDTO-s.</response>
        /// <response code="204">Returns no content.</response>
        [ProducesResponseType(typeof(List<ReservationDTO>), 200)]
        [ProducesResponseType(204)]
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

        /// <summary>
        /// [Authorized] User's reservations - which are older than 1 month - can be deleted (compared to checkout date).
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     GET /api/user/reservations/cleanup
        /// 
        /// </remarks>
        /// <returns>Returns No Content status.</returns>
        /// <response code="204">Returns no content.</response>
        [ProducesResponseType(typeof(Reservation), 200)]
        [ProducesResponseType(typeof(string), 400)]
        [Authorize(AuthenticationSchemes = authScheme, Roles = "User")]
        [HttpGet("user/reservations/cleanup")]
        public async Task<IActionResult> CleanUp()
        {
            await reservationService.CleanUp(GetUserId());
            return Ok("Old reservation have been removed.");
        }

        /// <summary>
        /// Forgotten password: User can request a password via email.
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     POST /api/user/resetPassword
        ///     {
        ///         "Email": "user1@bloodstone.com"
        ///     }
        ///     
        /// </remarks>
        /// <param name="passwordResetRequest"></param>
        /// <returns></returns>
        /// <response code="200">Returns a message: "Email sent".</response>
        /// <response code="400">Returns a list of error messages.</response>
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType(typeof(List<string>), 400)]
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

        /// <summary>
        /// [Authorized] User can logout.
        /// </summary>
        /// <remarks>
        /// 
        ///     GET /api/user/logout
        /// 
        /// </remarks>
        /// <returns>A message: "Signed out successfully.".</returns>
        /// <response code="200">Returns a string message.</response>
        [ProducesResponseType(typeof(string), 200)]
        [Authorize(AuthenticationSchemes = authScheme, Roles = "User")]
        [HttpGet("user/logout")]
        public async Task<IActionResult> Logout()
        {
            await accountService.SignOutAsync();
            return Ok("Signed out successfully.");
        }

        /// <summary>
        /// [Authorized] User can change their password.
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     POST /api/user/changePassword
        ///     {
        ///         "Password": "L14pJNkU4e",
        ///         "NewPassword": "Password66",
        ///         "VerifyPassword": "Password66"
        ///     }
        /// 
        /// </remarks>
        /// <param name="model"></param>
        /// <returns>A string message: "Password changed successfully.".</returns>
        /// <response code="200">Returns a string message: "Password changed successfully.".</response>
        /// <response code="400">Returns a list of string of validation errors.</response>
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType(typeof(List<string>), 400)]
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

        /// <summary>
        /// [Authorized] Fetch all reviews of a hotel. Max. 10/page.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /api/hotels/1/reviews
        ///     {
        ///         "CurrentPage": 1
        ///     }
        /// 
        /// </remarks>
        /// <param name="hotelId"></param>
        /// <param name="queryParams"></param>
        /// <returns></returns>
        /// <response code="200">Returns a paginated list of Review.</response>
        /// <response code="400">Returns a string error message.</response>
        [ProducesResponseType(typeof(PaginatedList<Review>), 200)]
        [ProducesResponseType(typeof(string), 400)]
        [Authorize(AuthenticationSchemes = authScheme, Roles = "User")]
        [HttpPost("hotels/{hotelId}/reviews")]
        public async Task<IActionResult> FindAllReviews(int hotelId, [FromBody] QueryParams queryParams)
        {
            var paginatedReviews = await hotelService.FindAllReviews(hotelId, queryParams);
            if (paginatedReviews.TotalPages < paginatedReviews.CurrentPage)
            {
                return BadRequest(string.Format("Error: the current page is greater than the number of pages. Max. page: {0}", paginatedReviews.TotalPages));
            }
            return Ok(paginatedReviews);
        }

        /// <summary>
        /// [Authorized] Add review to a hotel.
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     POST /api/hotels/2/addReview
        ///     {
        ///         "Rating": 3,
        ///         "Comment": "Very nice place!"
        ///     }
        /// 
        /// </remarks>
        /// <param name="hotelId"></param>
        /// <param name="review"></param>
        /// <returns>Returns a message: "Review added!".</returns>
        /// <response code="200">Returns a message: "Review added!".".</response>
        /// <response code="400">Returns validation errors of review.</response>
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType(typeof(List<string>), 400)]
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

        /// <summary>
        /// [Authorized] User can delete his review.
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     DETELE /api/hotels/1/review/24/delete
        /// 
        /// </remarks>
        /// <param name="hotelId"></param>
        /// <param name="reviewId"></param>
        /// <returns>Returns no content.</returns>
        /// <response code="204">Returns no content.</response>
        /// <response code="400">Returns string error message.</response>
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(string), 400)]
        [Authorize(AuthenticationSchemes = authScheme, Roles = "User")]
        [HttpDelete("hotels/{hotelId}/review/{reviewId}/delete")]
        public async Task<IActionResult> DeleteReview(int hotelId, int reviewId)
        {
            try
            {
                await hotelService.DeleteReview(reviewId, GetUserId());
            }
            catch (ItemNotFoundException e)
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