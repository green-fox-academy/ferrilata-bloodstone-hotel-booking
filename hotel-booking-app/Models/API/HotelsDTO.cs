using System.Collections.Generic;

namespace HotelBookingApp.Models.API
{
    public class HotelsDTO
    {
        public int PageCount { get; set; }
        public int CurrentPage { get; set; }
        public List<HotelDTO> Hotels { get; set; }
    }
}
