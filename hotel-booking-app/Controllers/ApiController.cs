using HotelBookingApp.Exceptions;
using HotelBookingApp.Models.Account;
using HotelBookingApp.Services;
using HotelBookingApp.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace HotelBookingApp.Controllers
{
    [Authorize]
    [Route("[controller]/")]
    [ApiController]
    public class ApiController : ControllerBase
    {
        private readonly IHotelService hotelService;
        private readonly IRoomService roomService;
        private readonly IAccountService accountService;

        public ApiController(IHotelService hotelService, IRoomService roomService, IAccountService accountService)
        {
            this.hotelService = hotelService;
            this.roomService = roomService;
            this.accountService = accountService;
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate([FromBody] LoginRequest request)
        {
            var response = await accountService.SignInApiAsync(request);
            if (response.token == null)
            {
                return BadRequest(string.Join(", ", response.errors.ToArray()));
            }
            return Ok(response);
        }

        [Authorize(Roles = "Admin, HotelManager")]
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

        [AllowAnonymous]
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
    }
}