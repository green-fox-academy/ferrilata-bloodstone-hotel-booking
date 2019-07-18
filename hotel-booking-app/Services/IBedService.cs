using HotelBookingApp.Models.HotelModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using HotelBookingApp.Pages;

namespace HotelBookingApp.Services
{
    public interface IBedService
    {
        Task<IEnumerable<SelectListItem>> FindAll();
        Task<Bed> FindBedById(int bedId);
        Task<RoomBed> AddBed(BedViewModel model);
        string GetBedTypesAsString(IEnumerable<RoomBed> roomBeds);
    }
}
