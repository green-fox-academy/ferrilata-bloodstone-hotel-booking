using HotelBookingApp.Models.Hotel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelBookingApp.Services
{
    public interface IHotelService
    {
        Task<IEnumerable<HotelModel>> FindAll();
        Task<IEnumerable<HotelModel>> FindAllOrderByName();
        Task<IEnumerable<HotelModel>> FindAllPaginated(string orderBy, int currentPage);
        Task Add(HotelModel hotel);
        Task Delete(long hotelId);
    }
}
