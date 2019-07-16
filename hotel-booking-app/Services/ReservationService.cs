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
        private readonly IEmailService emailService;

        public ReservationService(ApplicationContext context, IStringLocalizer<ReservationService> localizer, IEmailService emailService)
        {
            this.context = context;
            this.localizer = localizer;
            this.emailService = emailService;
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

        public async Task<Reservation> ConfirmAsync(int id, string userName)
        {
            var reservation = await FindByIdAsync(id);
            reservation.IsConfirmed = true;
            context.Update(reservation);
            await context.SaveChangesAsync();
            await emailService.SendMailAsync(reservation, userName);
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

        public async Task<IEnumerable<Reservation>> FindAllConfirmedByRoomIdAsync(int roomId)
        {
            return await context.Reservations
                .Where(r => r.Room.RoomId == roomId)
                .Where(r => r.IsConfirmed)
                .ToListAsync();
        }

        public async Task<IEnumerable<Reservation>> FindAllByUserId(string userId)
        {
            return await context.Reservations
                .Include(r => r.Room)
                .Include(r => r.ApplicationUser)
                .Where(r => r.ApplicationUserId == userId)
                .Where(r => r.IsConfirmed)
                .OrderBy(r => !r.IsCancelable)
                    .ThenBy(r => r.FromDate)
                .ToListAsync();
        }

        public async Task<bool> IsIntervalOccupied(Reservation reservation)
        {
            var confirmedReservations = await FindAllConfirmedByRoomIdAsync(reservation.RoomId);
            foreach (var cr in confirmedReservations)
            {
                if (cr.FromDate >= reservation.FromDate && cr.FromDate <= reservation.ToDate ||
                    cr.ToDate >= reservation.FromDate && cr.ToDate <= reservation.ToDate)
                {
                    return true;
                }
            }
            return false;
        }

        public async Task DeleteAsync(int id)
        {
            var reservation = await FindByIdAsync(id);
            if (reservation.IsCancelable)
            {
                context.Remove(reservation);
                await context.SaveChangesAsync();
            }
        }
    }
}
