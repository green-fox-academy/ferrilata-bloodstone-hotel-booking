using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HotelBookingApp.Models;

namespace HotelBookingApp.Services
{
    public class HotelService : IHotelService
    {
        public ApplicationContext applicationContext { get; }

        public HotelService(ApplicationContext applicationContext)
        {
            this.applicationContext = applicationContext;
        }


        public void AddHotel(HotelModel hotel)
        {
            applicationContext.Add(hotel);
            applicationContext.SaveChanges();
        }

        public void DeleteHotelById(long hotelId)
        {
            var hotel = applicationContext.Hotels.SingleOrDefault(h => h.Id == hotelId);
            applicationContext.Hotels.Remove(hotel);
            applicationContext.SaveChanges();
        }

        public IEnumerable<HotelModel> FindAll()
        {
            return applicationContext.Hotels.ToList();
        }

        public IEnumerable<HotelModel> ListAlphabetically()
        {
            return applicationContext.Hotels.OrderBy(h => h.Name);
        }
    }
}
