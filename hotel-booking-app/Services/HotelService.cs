using HotelBookingApp.Models.Hotel;
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

        public async Task Add(HotelModel hotel)
        {
            await applicationContext.AddAsync(hotel);
            await applicationContext.SaveChangesAsync();
        }

        public async Task Delete(long id)
        {
            var hotel = applicationContext.Hotels
                .SingleOrDefault(h => h.Id == id);
            applicationContext.Hotels.Remove(hotel);
            await applicationContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<HotelModel>> FindAll()
        {
            return await applicationContext.Hotels.ToListAsync();
        }

        public async Task<IEnumerable<HotelModel>> FindAllOrderByName()
        {
            return await applicationContext.Hotels
                .OrderBy(h => h.Name)
                .ToListAsync();
        }

        public async Task<IEnumerable<HotelModel>> FindAllPaginated(string orderBy, int currentPage)
        {
            var hotels = FindAllSorted(orderBy);
            return await PaginatedList<HotelModel>.CreateAsync(hotels, currentPage);
        }

        private IOrderedQueryable<HotelModel> FindAllSorted(string orderBy)
        {
            var hotels = applicationContext.Hotels;
            switch (orderBy.ToLower())
            {
                case "name":
                    return hotels.OrderBy(h => h.Name);
                case "description":
                    return hotels.OrderBy(h => h.Description);
                case "price":
                    return hotels.OrderBy(h => h.Price);
                case "id":
                    return hotels.OrderBy(h => h.Id);
                default:
                    return hotels.OrderBy(h => h.Name);
            }
        }
    }
}
