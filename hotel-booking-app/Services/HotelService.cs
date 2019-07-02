using HotelBookingApp.Exceptions;
using HotelBookingApp.Models.Hotel;
using HotelBookingApp.Utils;
using Microsoft.EntityFrameworkCore;
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

        public async Task Add(Hotel hotel)
        {
            await applicationContext.AddAsync(hotel);
            await applicationContext.SaveChangesAsync();
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

        public async Task<Hotel> FindById(long hotelId)
        {
            var hotel = await applicationContext.Hotels.FindAsync(hotelId);
            return hotel;
        }

        public async Task<PaginatedList<Hotel>> FindWithQuery(QueryParams queryParams)
        {
            var hotels = QueryableUtils<Hotel>.OrderCustom(applicationContext.Hotels, queryParams);
            return await PaginatedList<Hotel>.CreateAsync(hotels, queryParams.CurrentPage, queryParams.PageSize);
        }
    }
}
