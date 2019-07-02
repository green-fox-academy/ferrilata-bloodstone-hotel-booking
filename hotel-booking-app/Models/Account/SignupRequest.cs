using System.ComponentModel.DataAnnotations;

namespace HotelBookingApp.Models.Account
{
    public class SignupRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(20, MinimumLength = 6)]
        [RegularExpression("^(?=.*[A-Z])(?=.*[0-9])[A-Za-z0-9]*",
            ErrorMessage = "Your password must contain at least one upper case letter, one number and no special characters.")]
        public string Password { get; set; }

        [Compare("Password")]
        public string VerifyPassword { get; set; }
        public string ErrorMessage { get; set; }
    }
}
