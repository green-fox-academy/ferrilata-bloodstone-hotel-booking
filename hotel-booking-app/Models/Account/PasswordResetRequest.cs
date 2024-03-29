﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HotelBookingApp.Models.Account
{
    public class PasswordResetRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        public List<string> ErrorMessages { get; set; } = new List<string>();
        public List<string> SuccessMessages { get; set; } = new List<string>();
        
    }
}
