namespace HotelBookingApp.Models.HotelModels
{
    public class RoomBed
    {
        public int RoomId { get; set; }
        public int BedId { get; set; }
        public Room Room { get; set; }
        public Bed Bed { get; set; }
    }
}
