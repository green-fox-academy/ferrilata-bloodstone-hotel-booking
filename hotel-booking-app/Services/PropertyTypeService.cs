using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HotelBookingApp.Data;
using HotelBookingApp.Models.HotelModels;
using Microsoft.EntityFrameworkCore;

namespace HotelBookingApp.Services
{
    public class PropertyTypeService : IPropertyTypeService
    {
        private readonly ApplicationContext applicationContext;

        public PropertyTypeService(ApplicationContext applicationContext)
        {
            this.applicationContext = applicationContext;
        }

        public async Task<IEnumerable<PropertyType>> FindAll()
        {
            return await applicationContext.PropertyTypes.ToListAsync();
        }
    }
}
