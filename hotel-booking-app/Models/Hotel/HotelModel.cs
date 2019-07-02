using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelBookingApp.Models.Hotel
{
    public class HotelModel
    {
        public int HotelModelId { get; set; }
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
