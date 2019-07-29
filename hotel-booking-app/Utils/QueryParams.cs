namespace HotelBookingApp.Utils
{
    public class QueryParams
    {
        public string OrderBy { get; set; } = "Name";
        public bool Desc { get; set; } = false;
        public bool NextDesc { get { return !Desc; } }
        public int CurrentPage { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string Search { get; set; } = "";
        public int GuestNumber { get; set; } = 0;
    }
}
