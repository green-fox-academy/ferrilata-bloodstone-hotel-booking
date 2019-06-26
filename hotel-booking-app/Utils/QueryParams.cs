namespace HotelBookingApp.Utils
{
    public class QueryParams
    {
        public string OrderBy { get; set; } = "Name";
        public bool Desc { get; set; } = false;
        public int CurrentPage { get; set; } = 1;
    }
}
