using HotelBookingApp.Data;
using HotelBookingApp.Models.HotelModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelBookingApp.Services
{
    public class BedService : IBedService
    {
        private readonly ApplicationContext applicationContext;
        private readonly IStringLocalizer<BedService> localizer;

        public BedService(ApplicationContext applicationContext, IStringLocalizer<BedService> localizer)
        {
            this.applicationContext = applicationContext;
            this.localizer = localizer;
        }

        public async Task<IEnumerable<SelectListItem>> FindAll()
        {
            return await applicationContext.Beds
                .Select(b => new SelectListItem { Value = Convert.ToString(b.BedId), Text = localizer[b.Type] })
                .ToListAsync();
        }

        public async Task<Bed> FindBedById(int bedId)
        {
            var bed = await applicationContext.Beds.FindAsync(bedId);
            return bed;
        }
    }
}
