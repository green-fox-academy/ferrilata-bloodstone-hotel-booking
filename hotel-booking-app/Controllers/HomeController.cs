using HotelBookingApp.Pages;
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

        [HttpGet("/")]
        public async Task<IActionResult> Index(QueryParams queryParams)
        {
            return View(new IndexPageView
            {
                Hotels = await hotelService.FindWithQuery(queryParams),
                QueryParams = queryParams
            });
        }
    }
}
