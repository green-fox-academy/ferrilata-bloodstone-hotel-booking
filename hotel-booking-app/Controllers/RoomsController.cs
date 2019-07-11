using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
﻿using HotelBookingApp.Models.HotelModels;
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
        public IActionResult Add()
        {
            var beds = bedService.FindAll();
            var roomViewModel = new RoomViewModel
            {
                Beds = beds
            };
            return View(roomViewModel);
        }

        [HttpPost("new")]
        public async Task<IActionResult> Add(int hotelId, RoomViewModel model, List<int> bedsIdInRoom)
        {
            model.BedsIdInRoom = bedsIdInRoom;
            await hotelService.AddRoom(hotelId, model.Room, model.BedsIdInRoom);
            return RedirectToAction(nameof(HotelsController.Hotel), "Hotels", new { id = hotelId });
        }
    }
}