using HotelBookingApp.Models.Hotel;
using HotelBookingApp.Utils;
using System.Collections.Generic;

namespace HotelBookingApp.Pages
{
    public class IndexPageView
    {
        public IEnumerable<HotelModel> Hotels { get; set; }
        public QueryParams QueryParams { get; set; }
    }
}
