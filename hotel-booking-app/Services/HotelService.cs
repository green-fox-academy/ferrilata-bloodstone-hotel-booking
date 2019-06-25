using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HotelBookingApp.Models;

namespace HotelBookingApp.Services
{
    public class HotelService : IHotelService
    {
        public IEnumerable<HotelModel> FindAllPaginated(int currentPage = 1)
        {
            throw new NotImplementedException();
        }
    }
}
