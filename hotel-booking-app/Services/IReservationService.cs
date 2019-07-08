using HotelBookingApp.Models.HotelModels;
using System.Threading.Tasks;

namespace HotelBookingApp.Services
{
    public interface IReservationService
    {
        Task<Reservation> AddAsync(Reservation reservation);
        Task<Reservation> FindById(int id);
    }
}
