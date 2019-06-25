using System.ComponentModel.DataAnnotations;

namespace HotelBookingApp.Models.User
{
    public class UserLoginReq
    {
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
