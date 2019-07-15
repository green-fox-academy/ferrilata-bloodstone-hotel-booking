using HotelBookingApp.Models.HotelModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HotelBookingApp.Services
{
    public interface IReservationService
    {
        Task<Reservation> AddAsync(Reservation reservation);
        Task<Reservation> ConfirmAsync(int id);
        Task<Reservation> FindByIdAsync(int id);
        Task<IEnumerable<Reservation>> FindAllByHotelIdAsync(int hotelId);
        Task<IEnumerable<Reservation>> FindAllConfirmedByRoomIdAsync(int roomId);
        Task<IEnumerable<Reservation>> FindAllByUserId(string userId);
        Task<bool> IsIntervalOccupied(Reservation reservation);
        Task DeleteAsync(int id);
    }
}
