using HotelBookingApp.Models.HotelModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelBookingApp.Services
{
    public interface IRoomService
    {
        Task<Room> FindByIdAsync(int id);
        Task<Room> AddRoom(int hotelId, Room room);
        Task<Room> GetRoomWithAllProperties(int roomId);
    }
}
