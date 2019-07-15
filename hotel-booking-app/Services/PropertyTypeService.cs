using HotelBookingApp.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelBookingApp.Services
{
    public class PropertyTypeService : IPropertyTypeService
    {
        private readonly ApplicationContext applicationContext;
        private readonly IStringLocalizer<PropertyTypeService> localizer;

        public PropertyTypeService(ApplicationContext applicationContext, IStringLocalizer<PropertyTypeService> localizer)
        {
            this.applicationContext = applicationContext;
            this.localizer = localizer;
        }

        public async Task<IEnumerable<SelectListItem>> FindAll()
        {
            return await applicationContext.PropertyTypes
                .Select(pt => new SelectListItem { Value = Convert.ToString(pt.PropertyTypeId), Text = localizer[pt.Type] })
                .ToListAsync();
        }
    }
}
