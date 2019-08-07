using HotelBookingApp.Models.HotelModels;
using System.Collections.Generic;

namespace HotelBookingApp.Models.API
{
    public class RoomDTO
    {
        public int RoomId { get; set; }
        public string Name { get; set; }
        public int NumberOfBeds { get; set; }
        public int Capacity { get; set; }
        public double PricePerNight { get; set; }
    }
}
