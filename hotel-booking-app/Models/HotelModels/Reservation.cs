﻿using HotelBookingApp.Models.Account;

namespace HotelBookingApp.Models.HotelModels
{
    public class Reservation
    {
        public int ReservationId { get; set; }
        public int GuestNumber { get; set; } = 2;
        public string GuestNames { get; set; }
        public bool IsConfirmed { get; set; }
        public Room Room { get; set; }
        public int RoomId { get; set; }
        public ApplicationUser AppicationUser { get; set; }
        public string ApplicationUserId { get; set; }
    }
}
