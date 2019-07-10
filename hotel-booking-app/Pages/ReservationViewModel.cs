using HotelBookingApp.Models.HotelModels;
using System.Collections.Generic;

namespace HotelBookingApp.Pages
{
    public class ReservationViewModel
    {
        public int HotelId { get; set; }
        public int RoomId { get; set; }
        public int ReservationId { get; set; }
        public Reservation Reservation { get; set; } = new Reservation();
        public IEnumerable<Reservation> Reservations { get; set; }
    }
}
