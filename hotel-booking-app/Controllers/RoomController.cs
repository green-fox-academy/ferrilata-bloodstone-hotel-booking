using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HotelBookingApp.Models.HotelModels;
using HotelBookingApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelBookingApp.Controllers
{
    [Authorize]
    public class RoomController : Controller
    {
        private readonly IHotelService hotelService;

        public RoomController(IHotelService hotelService)
        {
            this.hotelService = hotelService;
        }

        [Authorize(Roles = "Admin, HotelManager")]
        [HttpGet("/hotel/{id}/room")]
        public IActionResult Add()
        {
            return View(new Room());
        }

        [Authorize(Roles = "Admin, HotelManager")]
        [HttpPost("/hotel/{id}/room")]
        public async Task<IActionResult> Add(int id, Room room)
        {
            var hotel = await hotelService.FindByIdAsync(id);
            await hotelService.AddRoom(id, room);
            return RedirectToAction(nameof(HotelsController.Hotel), "Hotels");
        }
    }
}
