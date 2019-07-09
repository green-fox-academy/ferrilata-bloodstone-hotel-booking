using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HotelBookingApp.Data;
using HotelBookingApp.Models.HotelModels;

namespace HotelBookingApp.Services
{
    public class ReservationService : IReservationService
    {
        private readonly ApplicationContext context;

        public ReservationService(ApplicationContext context)
        {
            this.context = context;
        }

        public async Task<Reservation> AddAsync(Reservation reservation)
        {
            await context.AddAsync(reservation);
            await context.SaveChangesAsync();
            return reservation;
        }

        public async Task<Reservation> Confirm(int id)
        {
            var reservation = await FindById(id);
            reservation.IsConfirmed = true;
            context.Update(reservation);
            await context.SaveChangesAsync();
            return reservation;
        }

        public async Task<Reservation> FindById(int id)
        {
            return await context.Reservations.FindAsync(id);
        }
    }
}
