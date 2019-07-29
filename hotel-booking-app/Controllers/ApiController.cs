using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HotelBookingApp.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HotelBookingApp.Controllers
{
    [Route("[controller]/")]
    [ApiController]
    public class ApiController : ControllerBase
    {
        private readonly IHotelService hotelService;

        public ApiController(IHotelService hotelService)
        {
            this.hotelService = hotelService;
        }

        [HttpGet("hotels")]
        public async Task<object> hotels(string city)
        {
            var paginatedHotels = await hotelService.FindWithQuery(new Utils.QueryParams { Search=city});
            return paginatedHotels.CurrentPage;
        }
    }
}