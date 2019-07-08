using HotelBookingApp.Models.HotelModels;
using System.Threading.Tasks;

namespace HotelBookingApp.Services
{
    public interface IRoomService
    {
        Task<Room> FindByIdAsync(int id);
    }
}
