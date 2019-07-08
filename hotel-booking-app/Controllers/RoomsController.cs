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

    [Authorize(Roles = "Admin, HotelManager")]
    public class RoomsController : Controller
    {
        private readonly IHotelService hotelService;

        public RoomsController(IHotelService hotelService)
        {
            this.hotelService = hotelService;
        }

        [HttpGet("/hotel/{id}/room/new")]
        public IActionResult Add()
        {
            return View(new Room());
        }

        [HttpPost("/hotel/{id}/room/new")]
        public async Task<IActionResult> Add(int id, Room room)
        {
            await hotelService.AddRoom(id, room);
            return RedirectToAction(nameof(HotelsController.Hotel), "Hotels");
        }
    }
}
