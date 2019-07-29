using HotelBookingApp.Models.HotelModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HotelBookingApp.Services
{
    public interface IRoomService
    {
        Task<Room> FindByIdAsync(int id);
        Task<Room> AddRoom(int hotelId, Room room);
        Task<Room> FindRoomWithAllProperties(int roomId);
        Task<List<ApiRoomDTO>> GetRoomDTOs(int hotelId);
    }
}
