using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HotelBookingApp.Models;
using Microsoft.EntityFrameworkCore;

namespace HotelBookingApp.Services
{
    public class HotelService : IHotelService
    {
        private ApplicationContext applicationContext { get; }

        public HotelService(ApplicationContext applicationContext)
        {
            this.applicationContext = applicationContext;
        }

        public async Task Add(HotelModel hotel)
        {
            await applicationContext.AddAsync(hotel);
            applicationContext.SaveChanges();
        }

        public void Delete(long id)
        {
            try
            { 
            var hotel = applicationContext.Hotels.SingleOrDefault(h => h.Id == id);
            applicationContext.Hotels.Remove(hotel);
            applicationContext.SaveChanges();
            }
            catch (KeyNotFoundException)
            {
                
            }
        }

        public async Task<IEnumerable<HotelModel>> FindAll()
        {
            return await applicationContext.Hotels.ToListAsync();
        }

        public async Task<IEnumerable<HotelModel>> FindAllOrderByName()
        {
            return await applicationContext.Hotels.OrderBy(h => h.Name).ToListAsync();
        }
    }
}
