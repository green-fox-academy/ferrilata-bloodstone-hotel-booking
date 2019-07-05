using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HotelBookingApp.Models.HotelModels
{
    public class Hotel
    {
        public int HotelId { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        [Required]
        public int Price { get; set; }
        [Required]
        public int StarRating { get; set; }
        //public List<IFormFile> ImageList { get; set; }

        public Location Location { get; set; }
        public int LocationId { get; set; }

        public PropertyType PropertyType { get; set; }
        public int PropertyTypeId { get; set; }

        public IEnumerable<Room> Rooms { get; set; }
    }
}
