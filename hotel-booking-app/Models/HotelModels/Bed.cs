using System.Collections.Generic;

namespace HotelBookingApp.Models.HotelModels
{
    public class Bed
    {
        public int BedId { get; set; }
        public string Type { get; set; }
        public int Size { get; set; }
        public IEnumerable<RoomBed> RoomBeds { get; set; }
    }
}
