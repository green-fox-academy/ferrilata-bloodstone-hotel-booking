using HotelBookingApp.Exceptions;
using HotelBookingApp.Models.HotelModels;
using HotelBookingApp.Pages;
using HotelBookingApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace HotelBookingApp.Controllers
{
    [Authorize]
    public class HotelsController : Controller
    {
        private readonly IHotelService hotelService;

        public HotelsController(IHotelService hotelService)
        {
            this.hotelService = hotelService;
        }

        [Authorize(Roles = "Admin, HotelManager")]
        [HttpGet("hotel/add")]
        public IActionResult Add()
        {
            return View(new HotelViewModel());
        }

        [Authorize(Roles = "Admin, HotelManager")]
        [HttpPost("hotel/add")]
        public async Task<IActionResult> Add(HotelViewModel model)
        {
            var hotel = await hotelService.Add(model.Hotel);
            return RedirectToAction(nameof(Hotel), new { id = hotel.HotelId });
        }

        [HttpGet("/hotel/{id}")]
        public async Task<IActionResult> Hotel(int id)
        {
            try
            {
                var hotel = await hotelService.FindByIdAsync(id);
                return View(hotel);
            }
            catch (ItemNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [Authorize(Roles = "Admin, HotelManager")]
        [HttpGet("/hotel/edit/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            var hotel = await hotelService.FindByIdAsync(id);
            return View(hotel);
        }

        [Authorize(Roles = "Admin, HotelManager")]
        [HttpPost("/hotel/edit/{id}")]
        public async Task<IActionResult> Edit(int id, Hotel hotel)
        {
            hotel.HotelId = id;
            await hotelService.Update(hotel);
            return RedirectToAction(nameof(Hotel), new { id } );
        }
    }
}
