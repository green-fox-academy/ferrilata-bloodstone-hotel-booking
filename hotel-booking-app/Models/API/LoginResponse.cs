using System.Collections.Generic;

namespace HotelBookingApp.Models.API
{
    public class LoginResponse
    {
        public List<string> errors {get; set; } = new List<string>();
        public string token {get; set; }
    }
}
