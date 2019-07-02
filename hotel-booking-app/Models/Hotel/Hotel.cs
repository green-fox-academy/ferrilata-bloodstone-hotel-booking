using System.Collections.Generic;

namespace HotelBookingApp.Models.Hotel
{
    public class Hotel
    {
        public int HotelId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }
        public int StarRating { get; set; }

        public Location Location { get; set; }
        public int LocationId { get; set; }

        public PropertyType PropertyType { get; set; }
        public int PropertyTypeId { get; set; }

        public IEnumerable<Room> Rooms { get; set; }
    }
}
