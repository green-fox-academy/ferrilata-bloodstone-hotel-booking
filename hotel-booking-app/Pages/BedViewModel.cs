using HotelBookingApp.Models.HotelModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelBookingApp.Pages
{
    public class BedViewModel
    {
        public IEnumerable<SelectListItem> Beds { get; set; }
        public int RoomId { get; set; }
        public int BedId { get; set; }
        public int BedNumber { get; set; }
        public int HotelId { get; set; }
    }
}
