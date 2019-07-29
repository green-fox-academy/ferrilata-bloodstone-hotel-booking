using HotelBookingApp.Models.HotelModels;
using System.Threading.Tasks;

namespace HotelBookingApp.Services
{
    public interface IRoomService
    {
        Task<Room> FindByIdAsync(int id);
        Task<Room> AddRoom(int hotelId, Room room);
        Task<Room> FindRoomWithAllProperties(int roomId);
        Task<object> GetRoomDTOs(int hotelId);
    }
}
