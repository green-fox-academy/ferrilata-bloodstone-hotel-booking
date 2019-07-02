using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelBookingApp.Models.Hotel
{
    public class Room
    {
        public int RoomId { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }

        public IEnumerable<RoomBed> RoomBeds { get; set; }
        public HotelModel Hotel { get; set; }
    }
}
