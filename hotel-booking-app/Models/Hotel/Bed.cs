﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelBookingApp.Models.Hotel
{
    public class Bed
    {
        public int BedId { get; set; }
        public string Type { get; set; }
        public int Size { get; set; }

        public IEnumerable<RoomBed> RoomBeds { get; set; }
    }
}
