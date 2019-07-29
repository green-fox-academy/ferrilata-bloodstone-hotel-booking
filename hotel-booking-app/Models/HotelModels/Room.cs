using System.Collections.Generic;
using System.Linq;

namespace HotelBookingApp.Models.HotelModels
{
    public class Room
    {
        public int RoomId { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public IEnumerable<RoomBed> RoomBeds { get; set; }
        public Hotel Hotel { get; set; }
        public int HotelId { get; set; }
        public int Capacity
        {
            get
            {
                int capacity = 0;
                if (RoomBeds != null)
                { 
                    capacity = RoomBeds.Sum(r => r.Bed.Size * r.BedNumber);
                }
                return capacity;
            }
        }
    }
}
