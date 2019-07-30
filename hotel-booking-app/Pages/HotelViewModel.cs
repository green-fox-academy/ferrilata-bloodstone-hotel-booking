using HotelBookingApp.Models.Image;
﻿using HotelBookingApp.Models.HotelModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using HotelBookingApp.Models.Account;

namespace HotelBookingApp.Pages
{
    public class HotelViewModel
    {
        public Hotel Hotel { get; set; }
        public Review Review { get; set; }
        public string CurrentUserId { get; set; }
        public IEnumerable<SelectListItem> PropertyTypes { get; set; }
        public List<ImageDetails> ImageList { get; set; }
    }
}
