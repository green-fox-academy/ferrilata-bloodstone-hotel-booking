namespace HotelBookingApp.Models.User
{
    public class UserSignupReq
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string VerifyPassword { get; set; }
    }
}
