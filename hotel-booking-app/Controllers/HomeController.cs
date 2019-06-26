﻿using HotelBookingApp.Exceptions;
using HotelBookingApp.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace HotelBookingApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHotelService hotelService;

        public HomeController(IHotelService hotelService)
        {
            this.hotelService = hotelService;
        }

        [Route("/")]
        public async Task<IActionResult> Index()
        {
            try
            {
                var hotels = await hotelService.FindAllOrderByName();
                return View(hotels);
            }
            catch
            {
                throw new ItemNotFoundException();
            }
        }
    }
}
