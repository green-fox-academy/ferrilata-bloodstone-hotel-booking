using HotelBookingApp.Data;
using HotelBookingApp.Models.HotelModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace HotelBookingApp.Services
{
    public class BedService : IBedService
    {
        private readonly ApplicationContext applicationContext;

        public BedService(ApplicationContext applicationContext)
        {
            this.applicationContext = applicationContext;
        }

        //public IEnumerable<Bed> FindAll()
        //{
        //    return applicationContext.Beds;
        //}

        public IEnumerable<SelectListItem> FindAll()
        {
            return  applicationContext.Beds
                .Select(b => new SelectListItem { Value = Convert.ToString(b.BedId), Text = b.Type })
                .ToList();
        }
    }
}
