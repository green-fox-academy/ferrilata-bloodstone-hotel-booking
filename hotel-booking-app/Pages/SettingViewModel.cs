using HotelBookingApp.Models.Account;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HotelBookingApp.Pages
{
    public class SettingViewModel
    {
        [Required]
        [StringLength(maximumLength: 20, MinimumLength = 6, ErrorMessage = "The field Password must be a string with a minimum length of '{2}' and a maximum length of '{1}'.")]
        [RegularExpression("^(?=.*[A-Z])(?=.*[0-9])[A-Za-z0-9]*",
            ErrorMessage = "Your password must contain at least one upper case letter, one number and no special characters.")]
        public string Password { get; set; }

        [Required]
        [StringLength(maximumLength: 20, MinimumLength = 6, ErrorMessage = "The field Password must be a string with a minimum length of '{2}' and a maximum length of '{1}'.")]
        [RegularExpression("^(?=.*[A-Z])(?=.*[0-9])[A-Za-z0-9]*",
            ErrorMessage = "Your password must contain at least one upper case letter, one number and no special characters.")]
        public string NewPassword { get; set; }

        [Compare("NewPassword", ErrorMessage = "The two password do not match.")]
        public string VerifyPassword { get; set; }

        public List<string> ErrorMessages { get; set; } = new List<string>();

        [JsonIgnore]
        public ApplicationUser ApplicationUser { get; set; }
        public string ApplicationUserId { get; set; }
    }
}
