using HotelBookingApp.Models.HotelModels;

namespace HotelBookingApp.Pages
{
    public class ReservationViewModel
    {
        public int HotelId { get; set; }
        public int RoomId { get; set; }
        public int ReservationId { get; set; }
        public Reservation Reservation { get; set; }
    }
}
