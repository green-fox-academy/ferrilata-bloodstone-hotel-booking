﻿using HotelBookingApp.Pages;
using HotelBookingApp.Services;
using HotelBookingApp.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace HotelBookingApp.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly IHotelService hotelService;

        public HomeController(IHotelService hotelService)
        {
            this.hotelService = hotelService;
        }

        [HttpGet("/")]
        public IActionResult Index()
        {
            return RedirectToAction(nameof(HotelsController.Index), "Hotels");
        }
    }
}
