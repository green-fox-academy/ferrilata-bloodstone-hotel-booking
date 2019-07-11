using HotelBookingApp.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelBookingApp.Services
{
    public class PropertyTypeService : IPropertyTypeService
    {
        private readonly ApplicationContext applicationContext;

        public PropertyTypeService(ApplicationContext applicationContext)
        {
            this.applicationContext = applicationContext;
        }

        public async Task<IEnumerable<SelectListItem>> FindAll()
        {
            return await applicationContext.PropertyTypes
                .Select(pt => new SelectListItem { Value = Convert.ToString(pt.PropertyTypeId), Text = pt.Type })
                .ToListAsync();
        }
    }
}
