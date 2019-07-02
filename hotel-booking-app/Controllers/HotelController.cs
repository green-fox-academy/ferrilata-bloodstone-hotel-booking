using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HotelBookingApp.Services;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HotelBookingApp.Controllers
{
    public class HotelController : Controller
    {
        private readonly IHotelService hotelService;

        public HotelController(IHotelService hotelService)
        {
            this.hotelService = hotelService;
        }

        [HttpGet("/hotel")]
        public IActionResult Hotel(long id)
        {
            var hotel = hotelService.FindById(id);
            return View(hotel);
        }
    }
}
