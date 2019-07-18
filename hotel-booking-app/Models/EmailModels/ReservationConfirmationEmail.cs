using HotelBookingApp.Models.HotelModels;

namespace HotelBookingApp.Models.EmailModels
{
    public class ReservationConfirmationEmail
    {
        public string BedTypes { get; set; }
        public Reservation Reservation { get; set; }
    }
}
