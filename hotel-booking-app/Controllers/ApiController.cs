using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using HotelBookingApp.Models.HotelModels;
using HotelBookingApp.Services;
using HotelBookingApp.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HotelBookingApp.Controllers
{
    [Route("[controller]/")]
    [ApiController]
    public class ApiController : ControllerBase
    {
        private readonly IHotelService hotelService;
        private readonly IMapper mapper;

        public ApiController(IHotelService hotelService, IMapper mapper)
        {
            this.hotelService = hotelService;
            this.mapper = mapper;
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
            var hotel = await hotelService.FindByIdAsync(hotelId);
            var roomList = new List<ApiRoomDTO>();
            foreach (var room in hotel.Rooms)
            {
                roomList.Add(mapper.Map<Room, ApiRoomDTO>(room));
            }
            return roomList;
        }
    }
}