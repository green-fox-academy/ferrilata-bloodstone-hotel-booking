using HotelBookingApp.Exceptions;
using HotelBookingApp.Models.HotelModels;
using HotelBookingApp.Pages;
using HotelBookingApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace HotelBookingApp.Controllers
{
    [Authorize]
    public class HotelsController : Controller
    {
        private readonly IHotelService hotelService;
        private readonly IPropertyTypeService propertyTypeService;

        public HotelsController(IHotelService hotelService, IPropertyTypeService propertyTypeService)
        {
            this.hotelService = hotelService;
            this.propertyTypeService = propertyTypeService;
        }

        [Authorize(Roles = "Admin, HotelManager")]
        [HttpGet("hotel/add")]
        public async Task<IActionResult> Add()
        {
            return View(new HotelViewModel {
                PropertyTypes = await propertyTypeService.FindAll()
            });
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
            return RedirectToAction(nameof(Hotel), new { id });
        }

        [Authorize(Roles = "Admin, HotelManager")]
        [HttpGet("/hotel/{id}/room")]
        public async Task<IActionResult> Room(int id)
        {
            var hotel = await hotelService.FindByIdAsync(id);
            return View(hotel);
        }

        [Authorize(Roles = "Admin, HotelManager")]
        [HttpPost("/hotel/{id}/room")]
        public async Task<IActionResult> Room(int id, string name, int price, string bedType, int bedSize)
        {           
            var room = new Room
            {
                Name = name,
                Price = price
            };

            var bed = new Bed
            {
                Type = bedType,
                Size = bedSize
            };

            var hotel = await hotelService.FindByIdAsync(id);
            await hotelService.AddRoom(id, room, bed);
            return View(hotel);
        }
    }
}
