using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HotelBookingApp.Models.HotelModels;
using HotelBookingApp.Pages;
using HotelBookingApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HotelBookingApp.Controllers
{
    [Authorize(Roles = "Admin, HotelManager")]
    [Route("hotels/{hotelId}/[controller]")]
    public class RoomsController : Controller
    {
        private readonly IHotelService hotelService;
        private readonly IBedService bedService;

        public RoomsController(IHotelService hotelService, IBedService bedService)
        {
            this.hotelService = hotelService;
            this.bedService = bedService;
        }

        [HttpGet("new")]
        public IActionResult Add(int hotelId)
        {
            var room = new Room
            {
                HotelId = hotelId
            };
            return View(room);
        }

        [HttpPost("new")]
        public async Task<IActionResult> Add(int hotelId, Room room)
        {
            await hotelService.AddRoom(hotelId, room);
            return RedirectToAction(nameof(HotelsController.Hotel), "Hotels", new { id = hotelId });
        }

        [HttpGet("{roomId}/bed/new")]
        public IActionResult AddBed(int hotelId, int roomId)
        {
            var beds = bedService.FindAll();
            var bedViewModel = new BedViewModel
            {
                Beds = beds,
                RoomId = roomId,
                HotelId = hotelId
            };
            return View(bedViewModel);
        }

        [HttpPost("{roomId}/bed/new")]
        public async Task<IActionResult> AddBed(int hotelId, BedViewModel model)
        {
            await hotelService.AddBed(model);
            return RedirectToAction(nameof(HotelsController.Hotel), "Hotels", new { id = hotelId });
        }
    }
}