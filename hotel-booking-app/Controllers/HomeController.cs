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
        public async Task<IActionResult> Index(string orderBy = "", int currentPage = 1)
        {
            var hotels = await hotelService.FindAllPaginated(orderBy, currentPage);
            return View(hotels);
        }
    }
}
