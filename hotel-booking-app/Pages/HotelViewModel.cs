using HotelBookingApp.Models.HotelModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace HotelBookingApp.Pages
{
    public class HotelViewModel
    {
        public Hotel Hotel { get; set; }
        public List<SelectListItem> PropertyTypes { get; set; }
    }
}
