using HotelBookingApp.Services;
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
        public async Task<IActionResult> Index(int currentPage = 1)
        {
            var hotels = await hotelService.FindAllPaginated(currentPage);
            return View(hotels);
        }
    }
}
