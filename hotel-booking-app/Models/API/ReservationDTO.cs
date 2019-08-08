using HotelBookingApp.Models.HotelModels;
using System;

namespace HotelBookingApp.Models.API
{
    public class ReservationDTO
    {
        public int ReservationId { get; set; }
        public int GuestNumber { get; set; } = 2;
        public string GuestNames { get; set; }
        public DateTime FromDate { get; set; } = DateTime.Now;
        public DateTime ToDate { get; set; } = DateTime.Now.AddDays(1);
        public bool IsConfirmed { get; set; }
        public RoomDTO Room { get; set; }
        public int RoomId { get; set; }
    }
}
