using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HotelBookingApp.Exceptions;
using HotelBookingApp.Services;
using Microsoft.AspNetCore.Mvc;

namespace HotelBookingApp.Controllers
{
    public class HotelsController : Controller
    {
        private readonly IHotelService hotelService;

        public HotelsController(IHotelService hotelService)
        {
            this.hotelService = hotelService;
        }

        [HttpGet("/hotel/{id}")]
        public async Task<IActionResult> Hotel(int id)
        {
            try
            {
                var hotel = await hotelService.FindByIdAsync(id);
                return View(hotel);
            }
            catch (ItemNotFoundException ex)
            {
                return NotFound(ex.Message);
            }

        }

        [HttpGet("/hotel/edit/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            var hotel = await hotelService.FindByIdAsync(id);
            return View(hotel);
        }

        [HttpGet("/hotel/{id}/room")]
        public async Task<IActionResult> Room(int id)
        {
            var hotel = await hotelService.FindByIdAsync(id);
            return View(hotel);
        }

    }

}
