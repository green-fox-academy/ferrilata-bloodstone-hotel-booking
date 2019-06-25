using HotelBookingApp.Models;
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
            return await applicationContext.Hotels
                    .ToListAsync();
        }

        public async Task<IEnumerable<HotelModel>> FindAllOrderByName()
        {
            return await applicationContext.Hotels
                    .OrderBy(h => h.Name)
                    .ToListAsync();
        }
    }
}
