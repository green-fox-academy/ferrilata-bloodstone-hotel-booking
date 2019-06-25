using System.ComponentModel.DataAnnotations;

namespace HotelBookingApp.Models.User
{
    public class UserSignupReq
    {
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string VerifyPassword { get; set; }
    }
}
