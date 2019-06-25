using System.ComponentModel.DataAnnotations;

namespace HotelBookingApp.Models.User
{
    public class UserSignupReq
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(30, MinimumLength = 6)]
        public string Password { get; set; }

        [Compare("Password")]
        public string VerifyPassword { get; set; }
    }
}
