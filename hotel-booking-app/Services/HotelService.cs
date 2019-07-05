using HotelBookingApp.Data;
using HotelBookingApp.Exceptions;
using HotelBookingApp.Models.HotelModels;
using HotelBookingApp.Utils;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelBookingApp.Services
{
    public class HotelService : IHotelService
    {
        private readonly ApplicationContext applicationContext;

        public HotelService(ApplicationContext applicationContext)
        {
            this.applicationContext = applicationContext;
        }

        public async Task<Hotel> Add(Hotel hotel)
        {
            await applicationContext.AddAsync(hotel);
            await applicationContext.SaveChangesAsync();
            return hotel;
        }

        public async Task Delete(long id)
        {
            var hotel = applicationContext.Hotels
                .SingleOrDefault(h => h.HotelId == id)
                ?? throw new ItemNotFoundException($"Hotel with id: {id} is not found.");
            applicationContext.Hotels.Remove(hotel);
            await applicationContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<Hotel>> FindAll()
        {
            return await applicationContext.Hotels.ToListAsync();
        }

        public async Task<IEnumerable<Hotel>> FindAllOrderByName()
        {
            return await applicationContext.Hotels
                .OrderBy(h => h.Name)
                .ToListAsync();
        }

        public async Task<Hotel> FindByIdAsync(int id)
        {
            var hotel = await applicationContext.Hotels
                .Include(h => h.Location)
                .Include(h => h.PropertyType)
                .Include(h => h.Rooms)
                    .ThenInclude(r => r.RoomBeds)
                        .ThenInclude(b => b.Bed)
                .SingleOrDefaultAsync(h => h.HotelId == id)
                ?? throw new ItemNotFoundException($"Hotel with id: {id} is not found.");
            return hotel;
        }


        public async Task<PaginatedList<Hotel>> FindWithQuery(QueryParams queryParams)
        {
            var filteredHotels = applicationContext.Hotels
                .Include(h => h.Location)
                .Where(h =>
                    string.IsNullOrEmpty(queryParams.Search)
                    || h.Location.City.Contains(queryParams.Search));

            var orderedHotels = QueryableUtils<Hotel>.OrderCustom(filteredHotels, queryParams);
            return await PaginatedList<Hotel>.CreateAsync(orderedHotels, queryParams.CurrentPage, queryParams.PageSize);
        }

        public async Task<Hotel> Update(Hotel hotel)
        {
            applicationContext.Update(hotel);
            await applicationContext.SaveChangesAsync();
            return hotel;
        }

        public async Task AddRoom(int hotelId, Room room)
        {
            var hotel = await applicationContext.Hotels.FindAsync(hotelId);
            room.Hotel = hotel;
            await applicationContext.AddAsync(room);
            //hotel.Rooms.ToList().Add(room);
            await applicationContext.SaveChangesAsync();
        }
    }
}
