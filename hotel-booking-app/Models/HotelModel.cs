using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelBookingApp.Models
{
    public class HotelModel
    {
        public long Id { get; set; }
        public string  Name { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }

        public HotelModel(long id, string name, string description, int price)
        {
            Id = id;
            Name = name;
            Description = description;
            Price = price;
        }

        public HotelModel()
        {
        }
    }
}
