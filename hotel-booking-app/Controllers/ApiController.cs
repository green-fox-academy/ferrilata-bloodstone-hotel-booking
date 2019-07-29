using AutoMapper;
using HotelBookingApp.Services;
using HotelBookingApp.Utils;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace HotelBookingApp.Controllers
{
    [Route("[controller]/")]
    [ApiController]
    public class ApiController : ControllerBase
    {
        private readonly IHotelService hotelService;
        private readonly IMapper mapper;
        private readonly IRoomService roomService;

        public ApiController(IHotelService hotelService, IMapper mapper, IRoomService roomService)
        {
            this.hotelService = hotelService;
            this.mapper = mapper;
            this.roomService = roomService;
        }

        [HttpGet("hotels")]
        public async Task<object> Hotels(string city, int currentPage = 1)
        {
            var paginatedHotels = await hotelService.FindWithQuery(new QueryParams
            {
                CurrentPage = currentPage,
                Search = city
            });
            return hotelService.GetHotelDTOs(paginatedHotels);
        }

        [HttpGet("hotels/{hotelId}/rooms")]
        public async Task<object> Rooms(int hotelId)
        {
            return await roomService.GetRoomDTOs(hotelId);
        }
    }
}