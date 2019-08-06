namespace HotelBookingApp.Models.HotelModels
{
    public class ApiHotelDTO
    {
        public int HotelId { get; set; }
        public string Name { get; set; }
        public string ShortDescription { get; set; }
        public int StarRating { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
        public string ThumbnailUrl { get; set;}
    }
}
