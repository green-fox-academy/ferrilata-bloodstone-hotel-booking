using HotelBookingApp.Models.HotelModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelBookingApp.Services
{
    public interface ILocationService
    {
        Task Add(Location location);
    }
}
