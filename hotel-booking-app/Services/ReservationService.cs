using HotelBookingApp.Data;
using HotelBookingApp.Exceptions;
using HotelBookingApp.Models.HotelModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelBookingApp.Services
{
    public class ReservationService : IReservationService
    {
        private readonly ApplicationContext context;
        private readonly IStringLocalizer<ReservationService> localizer;

        public ReservationService(ApplicationContext context, IStringLocalizer<ReservationService> localizer)
        {
            this.context = context;
            this.localizer = localizer;
        }

        public async Task<Reservation> AddAsync(Reservation reservation)
        {
            if (reservation.ReservationId != 0)
            {
                context.Update(reservation);
            } else
            {
                await context.AddAsync(reservation);
            }
            await context.SaveChangesAsync();
            return reservation;
        }

        public async Task<Reservation> ConfirmAsync(int id)
        {
            var reservation = await FindByIdAsync(id);
            reservation.IsConfirmed = true;
            context.Update(reservation);
            await context.SaveChangesAsync();
            return reservation;
        }

        public async Task<Reservation> FindByIdAsync(int id)
        {
            return await context.Reservations
                .Include(r => r.Room)
                .FirstOrDefaultAsync(r => r.ReservationId == id)
                ?? throw new ItemNotFoundException(localizer["Reservation with id {0} is not found!", id]);
        }

        public async Task<IEnumerable<Reservation>> FindAllByHotelIdAsync(int hotelId)
        {
            return await context.Reservations
                .Include(r => r.Room)
                .Include(r => r.ApplicationUser)
                .Where(r => r.Room.HotelId == hotelId)
                .ToListAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var reservation = FindByIdAsync(id);
            context.Remove(reservation);
            await context.SaveChangesAsync();
        }
    }
}
