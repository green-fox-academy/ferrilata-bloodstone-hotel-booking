using HotelBookingApp.Models.HotelModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HotelBookingApp.Pages
{
    public class RoomViewModel
    {
        public Room Room { get; set; }
        public IEnumerable<Bed> Beds { get; set; }
    }
}
