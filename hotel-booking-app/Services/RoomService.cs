using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HotelBookingApp.Data;
using HotelBookingApp.Models.HotelModels;

namespace HotelBookingApp.Services
{
    public class RoomService : IRoomService
    {
        private readonly ApplicationContext context;

        public RoomService(ApplicationContext context)
        {
            this.context = context;
        }

        public async Task<Room> FindByIdAsync(int id)
        {
            return await context.Rooms.FindAsync(id);
        }

        public async Task<Room> AddRoom(int hotelId, Room room)
        {
            room.HotelId = hotelId;
            await context.AddAsync(room);
            await context.SaveChangesAsync();
            return room;
        }
    }
}
