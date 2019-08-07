using HotelBookingApp.Models.Account;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;

namespace HotelBookingApp.Models.HotelModels
{
    public class Reservation
    {
        public int ReservationId { get; set; }

        [Required]
        [Range(1, 30)]
        [Remote(action: "VerifyGuestNumber", controller: "Reservations", AdditionalFields = nameof(RoomId))]
        public int GuestNumber { get; set; } = 2;

        [Required]
        [Remote(action: "VerifyGuestNames", controller: "Reservations", AdditionalFields = nameof(GuestNumber))]
        public string GuestNames { get; set; }

        [DataType(DataType.Date)]
        [Remote(action: "VerifyFromDate", controller: "Reservations", 
            AdditionalFields = nameof(ToDate) + "," + nameof(GuestNumber) + "," + nameof(RoomId))]
        public DateTime FromDate { get; set; } = DateTime.Now;

        [DataType(DataType.Date)]
        [Remote(action: "VerifyToDate", controller: "Reservations", AdditionalFields = nameof(FromDate))]
        public DateTime ToDate { get; set; } = DateTime.Now.AddDays(1);

        public bool IsConfirmed { get; set; }
        public Room Room { get; set; }
        public int RoomId { get; set; }
        [JsonIgnore]
        public ApplicationUser ApplicationUser { get; set; }
        public string ApplicationUserId { get; set; }

        public int NumberOfNights { get => (ToDate - FromDate).Days; }
        public bool IsCancelable { get => FromDate >= DateTime.Now.AddDays(1); }
    }
}
