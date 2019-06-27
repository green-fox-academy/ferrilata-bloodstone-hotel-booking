using HotelBookingApp.Services;
using HotelBookingApp.Utils;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace HotelBookingApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHotelService hotelService;

        public HomeController(IHotelService hotelService)
        {
            this.hotelService = hotelService;
        }

        [Route("/")]
        public async Task<IActionResult> Index(QueryParams queryParams)
        {
            var hotels = await hotelService.FindWithQuery(queryParams);
            ViewBag.NextDesc = !queryParams.Desc;
            return View(hotels);
        }
    }
}
