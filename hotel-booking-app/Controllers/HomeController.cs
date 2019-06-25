using Microsoft.AspNetCore.Mvc;
namespace HotelBookingApp.Controllers
{
    public class HomeController : Controller
    {
        [Route("/")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
