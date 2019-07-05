using HotelBookingApp.Models.Hotel;
using HotelBookingApp.Utils;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HotelBookingApp.Services
{
    public interface IHotelService
    {
        Task<IEnumerable<Hotel>> FindAll();
        Task<IEnumerable<Hotel>> FindAllOrderByName();
        Task<PaginatedList<Hotel>> FindWithQuery(QueryParams queryParams);
        Task Add(Hotel hotel);
        Task Delete(long hotelId);
        Task<Hotel> FindByIdAsync(int id);
        Task<Hotel> Update(Hotel hotel);
        Task AddRoom(int id, Room room, Bed bed);
    }
}
