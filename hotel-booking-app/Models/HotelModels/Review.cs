using HotelBookingApp.Models.Account;

namespace HotelBookingApp.Models.HotelModels
{
    public class Review
    {
        public int ReviewId { get; set; }
        public string Comment { get; set; }
        public int Rating { get; set; }
        public Hotel Hotel { get; set; }
        public int HotelId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public string ApplicationUserId { get; set; }
    }
}
