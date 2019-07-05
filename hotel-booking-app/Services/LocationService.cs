using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HotelBookingApp.Data;
using HotelBookingApp.Models.HotelModels;

namespace HotelBookingApp.Services
{
    public class LocationService : ILocationService
    {
        private readonly ApplicationContext applicationContext;

        public LocationService(ApplicationContext applicationContext)
        {
            this.applicationContext = applicationContext;
        }

        public async Task Add(Location location)
        {
            await applicationContext.AddAsync(location);
            await applicationContext.SaveChangesAsync();
        }
    }
}
