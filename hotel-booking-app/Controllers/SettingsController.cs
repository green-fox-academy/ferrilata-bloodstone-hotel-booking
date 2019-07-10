using HotelBookingApp.Pages;
using HotelBookingApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace HotelBookingApp.Controllers
{
    [Authorize]
    public class SettingsController : Controller
    {
        [HttpGet("Settings")]
        public IActionResult Settings()
        {
            return View();
        }
    }
}
