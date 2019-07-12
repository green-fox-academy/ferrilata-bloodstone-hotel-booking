using HotelBookingApp.Data;
using HotelBookingApp.Models.HotelModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
//using System.Data.Entity;
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

        public IEnumerable<SelectListItem> FindAll()
        {
            return applicationContext.Beds
                .Select(b => new SelectListItem { Value = Convert.ToString(b.BedId), Text = localizer[b.Type] })
                .ToList();
        }
    }
}
