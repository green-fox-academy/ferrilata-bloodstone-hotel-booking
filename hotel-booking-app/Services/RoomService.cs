using AutoMapper;
using HotelBookingApp.Data;
using HotelBookingApp.Exceptions;
using HotelBookingApp.Models.HotelModels;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelBookingApp.Services
{
    public class RoomService : IRoomService
    {
        private readonly ApplicationContext context;
        private readonly IMapper mapper;
        private readonly IHotelService hotelService;

        public RoomService(ApplicationContext context, IMapper mapper, IHotelService hotelService)
        {
            this.context = context;
            this.mapper = mapper;
            this.hotelService = hotelService;
        }

        public async Task<Room> FindByIdAsync(int id)
        {
            return await context.Rooms
                .Include(r => r.RoomBeds)
                    .ThenInclude(rb => rb.Bed)
                .SingleOrDefaultAsync(r => r.RoomId == id);
        }

        public async Task<Room> AddRoom(int hotelId, Room room)
        {
            room.HotelId = hotelId;
            await context.AddAsync(room);
            await context.SaveChangesAsync();
            return room;
        }

        public async Task<Room> FindRoomWithAllProperties(int roomId)
        {
            return await context.Rooms
                .Include(room => room.Hotel)
                    .ThenInclude(hotel => hotel.Location)
                .Include(room => room.RoomBeds)
                    .ThenInclude(roomBed => roomBed.Bed)
                .Where(room => room.RoomId == roomId)
                .FirstOrDefaultAsync() ?? throw new ItemNotFoundException("Room with the provided id not found.");
        }

        public async Task<List<ApiRoomDTO>> GetRoomDTOs(int hotelId)
        {
            var hotel = await hotelService.FindByIdAsync(hotelId);
            return mapper.Map<List<Room>, List<ApiRoomDTO>>(hotel.Rooms.ToList());
        }
    }
}
