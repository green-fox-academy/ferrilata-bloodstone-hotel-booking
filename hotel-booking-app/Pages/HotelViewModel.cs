using HotelBookingApp.Models.Hotel;
using HotelBookingApp.Models.Image;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace HotelBookingApp.Pages
{
    public class HotelViewModel
    {
        public Hotel Hotel { get; set; }
        public List<SelectListItem> PropertyTypes { get; set; }
        public List<ImageDetails> ImageList { get; set; }
    }
}