namespace HotelBookingApp.Models.HotelModels
{
    public class ApiRoomDTO
    {
        public int RoomId { get; set; }
        public string Name { get; set; }
        public int NumberOfBeds { get; set; }
        public int Capacity { get; set; }
        public double Price { get; set; }
    }
}
