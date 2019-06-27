using HotelBookingApp.Models.Hotel;
using HotelBookingApp.Utils;

namespace HotelBookingApp.Pages
{
    public class IndexPageView
    {
        public PaginatedList<HotelModel> Hotels { get; set; }
        public QueryParams QueryParams { get; set; }
    }
}
