using HotelBookingApp.Models.HotelModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HotelBookingApp.Services
{
    public interface IBedService
    {
        IEnumerable<SelectListItem> FindAll();
    }
}
