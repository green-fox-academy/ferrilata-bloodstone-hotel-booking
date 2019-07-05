namespace HotelBookingApp.Models.HotelModels
{
    public class Location
    {
        public int LocationId { get; set; }
        public string Country { get; set; }
        public string Region { get; set; }
        public string City { get; set; }
        public string Address { get; set; }

        public Hotel Hotel { get; set; }
    }
}
