using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelBookingApp.Models.Hotel
{
    public class RoomBed
    {
        public int RoomId { get; set; }
        public int BedId { get; set; }
        public Room Room { get; set; }
        public Bed Bed { get; set; }
    }
}
