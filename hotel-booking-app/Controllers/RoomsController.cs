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
    [Route("hotels/{hotelId}/[controller]")]
    public class RoomsController : Controller
    {
        private readonly IRoomService roomService;
        private readonly IBedService bedService;
        private readonly IHotelService hotelService;

        public RoomsController(IBedService bedService, IRoomService roomService, IHotelService hotelService)
        {
            this.roomService = roomService;
            this.bedService = bedService;
            this.hotelService = hotelService;
        }

        [HttpGet("new")]
        public IActionResult Add(int hotelId)
        {
            var room = new Room { HotelId = hotelId };
            if (room.Hotel.ApplicationUserId == User.FindFirstValue(ClaimTypes.NameIdentifier) || User.IsInRole("Admin"))
            {
                return View(new Room { HotelId = hotelId });
            }
            return RedirectToAction(nameof(AccountController.AccessDenied));
        }

        [HttpPost("new")]
        public async Task<IActionResult> Add(int hotelId, Room room)
        {
            await roomService.AddRoom(hotelId, room);
            return RedirectToAction(nameof(HotelsController.Hotel), "Hotels", new { id = hotelId });
        }

        [HttpGet("{roomId}/beds/new")]
        public async Task<IActionResult> AddBed(int hotelId, int roomId)
        {
            var beds = await bedService.FindAll();
            var hotel = await hotelService.FindByIdAsync(hotelId);
            if (hotel.ApplicationUserId == User.FindFirstValue(ClaimTypes.NameIdentifier) || User.IsInRole("Admin"))
            {
                return View(new BedViewModel
                {
                    Beds = beds,
                    RoomId = roomId,
                    HotelId = hotelId
                });
            }
            return RedirectToAction(nameof(AccountController.AccessDenied));
        }

        [HttpPost("{roomId}/beds/new")]
        public async Task<IActionResult> AddBed(int hotelId, BedViewModel model)
        {
            await bedService.AddBed(model);
            return RedirectToAction(nameof(HotelsController.Hotel), "Hotels", new { id = hotelId });
        }
    }
}