using HotelBookingApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelBookingApp.Services
{
    public interface IHotelService
    {
        IEnumerable<HotelModel> FindAll();
        IEnumerable<HotelModel> ListAlphabetically();
        void AddHotel();
        void DeleteHotel();
    }
}
