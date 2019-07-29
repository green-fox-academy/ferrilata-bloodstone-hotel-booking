using HotelBookingApp.Exceptions;
using HotelBookingApp.Models.HotelModels;
using HotelBookingApp.Pages;
using HotelBookingApp.Services;
using HotelBookingApp.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace HotelBookingApp.Controllers
{
    [Authorize]
    [Route("[controller]")]
    public class HotelsController : Controller
    {
        private readonly IHotelService hotelService;
        private readonly IImageService imageService;
        private readonly IThumbnailService thumbnailService;
        private readonly IPropertyTypeService propertyTypeService;

        public HotelsController(IHotelService hotelService, IImageService imageService, IThumbnailService thumbnailService, IPropertyTypeService propertyTypeService)
        {
            this.hotelService = hotelService;
            this.imageService = imageService;
            this.thumbnailService = thumbnailService;
            this.propertyTypeService = propertyTypeService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(QueryParams queryParams)
        {
            return View(new IndexPageView
            {
                Hotels = await hotelService.FindWithQuery(queryParams),
                QueryParams = queryParams,
                ActionName = nameof(Index)
            });
        }

        [Authorize(Roles = "Admin, HotelManager")]
        [HttpGet("my-hotels")]
        public async Task<IActionResult> MyHotels(QueryParams queryParams)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            return View(nameof(Index), new IndexPageView
            {
                Hotels = await hotelService.FindWithQuery(queryParams, userId),
                QueryParams = queryParams,
                ActionName = nameof(MyHotels)
            });
        }

        [Authorize(Roles = "Admin, HotelManager")]
        [HttpGet("add")]
        public async Task<IActionResult> Add()
        {
            return View(new HotelViewModel
            {
                PropertyTypes = await propertyTypeService.FindAll()
            });
        }

        [Authorize(Roles = "Admin, HotelManager")]
        [HttpPost("add")]
        public async Task<IActionResult> Add(HotelViewModel model, List<IFormFile> imageList)
        {
            if (!ModelState.IsValid)
            {
                model.PropertyTypes = await propertyTypeService.FindAll();
                return View(model);
            }
            model.Hotel.ApplicationUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var hotel = await hotelService.Add(model.Hotel);
            await imageService.UploadImagesAsync(imageList, hotel.HotelId);
            return RedirectToAction(nameof(Hotel), new { id = hotel.HotelId });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Hotel(int id)
        {
            try
            {
                return View(new HotelViewModel
                {
                    Hotel = await hotelService.FindByIdAsync(id),
                    ImageList = await imageService.GetImageListAsync(id),
                    Review = new Review()
                });
            }
            catch (ItemNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [Authorize(Roles = "Admin, HotelManager")]
        [HttpGet("edit/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            return View(new HotelViewModel
            {
                Hotel = await hotelService.FindByIdAsync(id),
                ImageList = await imageService.GetImageListAsync(id),
                PropertyTypes = await propertyTypeService.FindAll()
            });
        }

        [Authorize(Roles = "Admin, HotelManager")]
        [HttpPost("edit/{id}")]
        public async Task<IActionResult> Edit(int id, Hotel hotel, List<IFormFile> imageList)
        {
            hotel.HotelId = id;
            await hotelService.Update(hotel);
            await imageService.UploadImagesAsync(imageList, id);
            return RedirectToAction(nameof(Hotel), new { id });
        }

        [Authorize(Roles = "Admin, HotelManager")]
        [HttpPost("{id}/images/delete")]
        public async Task<IActionResult> DeleteImage(int id, string path)
        {
            await imageService.DeleteFileAsync(path);
            return RedirectToAction(nameof(Hotel), new { id });
        }

        [Authorize(Roles = "Admin, HotelManager")]
        [HttpPost("{id}/thumbnail/update")]
        public async Task<IActionResult> UpdateThumbnail(int id, string path)
        {
            await thumbnailService.UpdateThumbnailFromUrl(id, path);
            return RedirectToAction(nameof(Hotel), new { id });
        }

        [Authorize(Roles = "Admin, HotelManager")]
        [HttpPost("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await hotelService.Delete(id);
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        [Authorize(Roles = "Admin, User")]
        [HttpPost("add-review/{id}")]
        public async Task<IActionResult> AddReview(int id, Review review)
        {
            if (!ModelState.IsValid)
            {
                return View(nameof(Hotel), new { id });
            }
            review.ApplicationUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            await hotelService.AddReviewAsync(review);
            return RedirectToAction(nameof(Hotel), new { id });
        }
    }
}

