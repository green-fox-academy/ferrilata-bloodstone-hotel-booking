using HotelBookingApp.Models.HotelModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelBookingApp.Services
{
    public interface IPropertyTypeService
    {
        Task<IEnumerable<SelectListItem>> FindAll();
    }
}
