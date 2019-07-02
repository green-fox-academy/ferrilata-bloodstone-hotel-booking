using System.ComponentModel.DataAnnotations;

namespace HotelBookingApp.Models.Account
{
    public class LoginRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        public string ErrorMessage { get; set; }
    }
}
