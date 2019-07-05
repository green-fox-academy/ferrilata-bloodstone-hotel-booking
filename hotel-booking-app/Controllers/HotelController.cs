using HotelBookingApp.Pages;
using HotelBookingApp.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace HotelBookingApp.Controllers
{
    public class HotelController : Controller
    {
        private readonly IHotelService hotelService;
        private readonly ILocationService locationService;

        public HotelController(IHotelService hotelService, ILocationService locationService)
        {
            this.hotelService = hotelService;
            this.locationService = locationService;
        }
        [HttpGet("hotel/add")]
        public IActionResult Add()
        {
            return View(new HotelViewModel());
        }

        [HttpPost("hotel/add")]
        public async Task<IActionResult> Add(HotelViewModel model
            )
        {
            var Id = await hotelService.Add(model.Hotel);
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }
    }
}
