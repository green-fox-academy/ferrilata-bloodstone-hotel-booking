using System.Collections.Generic;

namespace HotelBookingApp.Models.HotelModels
{
    public class ApiHotelsDTO
    {
        public int PageCount { get; set; }
        public int CurrentPage { get; set; }
        public List<ApiHotelDTO> Hotels { get; set; }
    }
}
