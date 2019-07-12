using HotelBookingApp.Models.HotelModels;
using HotelBookingApp.Pages;
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
        Task<Hotel> Add(Hotel hotel);
        Task Delete(int hotelId);
        Task<Hotel> FindByIdAsync(int id);
        Task<Hotel> Update(Hotel hotel);
        Task<Room> AddRoom(int hotelId, Room room);
        Task<Bed> FindBedById(int bedId);
        Room FindRoomById(int roomId);
        Task<RoomBed> AddBed(BedViewModel model);
    }
}
