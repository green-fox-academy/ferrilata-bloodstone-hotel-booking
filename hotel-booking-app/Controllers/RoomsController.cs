using HotelBookingApp.Models.HotelModels;
using HotelBookingApp.Pages;
using HotelBookingApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace HotelBookingApp.Controllers
{
    [Authorize(Roles = "Admin, HotelManager")]
    [Route("hotels/{hotelId}")]
    public class RoomsController : Controller
    {
        private readonly IHotelService hotelService;

        public RoomsController(IHotelService hotelService)
        {
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
    }
}