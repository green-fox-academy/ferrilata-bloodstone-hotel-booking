using HotelBookingApp.Models.Account;
using System;
using System.ComponentModel.DataAnnotations;

namespace HotelBookingApp.Models.HotelModels
{
    public class Reservation
    {
        public int ReservationId { get; set; }

        [Range(1, 20)]
        public int GuestNumber { get; set; } = 2;

        [Required]
        [StringLength(50, MinimumLength = 3)]
        public string GuestNames { get; set; }
        public bool IsConfirmed { get; set; }

        [DataType(DataType.Date)]
        public DateTime FromDate { get; set; } = DateTime.Now;

        [DataType(DataType.Date)]
        public DateTime ToDate { get; set; } = DateTime.Now;
        public Room Room { get; set; }
        public int RoomId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public string ApplicationUserId { get; set; }
    }
}
