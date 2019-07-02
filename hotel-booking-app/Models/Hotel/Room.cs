using System.Collections.Generic;

namespace HotelBookingApp.Models.Hotel
{
    public class Room
    {
        public int RoomId { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }

        public IEnumerable<RoomBed> RoomBeds { get; set; }
        public Hotel Hotel { get; set; }
    }
}
