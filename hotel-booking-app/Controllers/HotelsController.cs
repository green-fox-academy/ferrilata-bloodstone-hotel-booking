using HotelBookingApp.Exceptions;
using HotelBookingApp.Models.HotelModels;
using HotelBookingApp.Pages;
using HotelBookingApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HotelBookingApp.Controllers
{
    [Authorize]
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

        [Authorize(Roles = "Admin, HotelManager")]
        [HttpGet("hotel/add")]
        public async Task<IActionResult> Add()
        {
            return View(new HotelViewModel
            {
                PropertyTypes = await propertyTypeService.FindAll()
            });
        }

        [Authorize(Roles = "Admin, HotelManager")]
        [HttpPost("hotel/add")]
        public async Task<IActionResult> Add(HotelViewModel model, List<IFormFile> imageList)
        {
            var hotel = await hotelService.Add(model.Hotel);
            await imageService.UploadImagesAsync(imageList, hotel.HotelId);
            return RedirectToAction(nameof(Hotel), new { id = hotel.HotelId });
        }

        [HttpGet("/hotel/{id}")]
        public async Task<IActionResult> Hotel(int id)
        {
            try
            {
                var hotel = await hotelService.FindByIdAsync(id);
                var images = await imageService.GetImageListAsync(id);
                var model = new HotelViewModel
                {
                    Hotel = hotel,
                    ImageList = images
                };
                return View(model);
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
            return View(new HotelViewModel
            {
                Hotel = await hotelService.FindByIdAsync(id),
                ImageList = await imageService.GetImageListAsync(id),
                PropertyTypes = await propertyTypeService.FindAll()
            });
        }

        [Authorize(Roles = "Admin, HotelManager")]
        [HttpPost("/hotel/edit/{id}")]
        public async Task<IActionResult> Edit(int id, Hotel hotel, List<IFormFile> imageList)
        {
            hotel.HotelId = id;
            await hotelService.Update(hotel);
            await imageService.UploadImagesAsync(imageList, id);
            return RedirectToAction(nameof(Hotel), new { id } );
        }

        [Authorize(Roles = "Admin, HotelManager")]
        [HttpPost("/hotel/{id}/images/delete")]
        public async Task<IActionResult> DeleteImage(int id, string path)
        {
            await imageService.DeleteFileAsync(path);
            return RedirectToAction(nameof(Hotel), new { id });
        }

        [Authorize(Roles = "Admin, HotelManager")]
        [HttpPost("/hotel/{id}/thumbnail/update")]
        public async Task<IActionResult> UpdateThumbnail(int id, string path)
        {
            await thumbnailService.UpdateThumbnailFromUrl(id, path);
            return RedirectToAction(nameof(Hotel), new { id });
        }

        [Authorize(Roles = "Admin, HotelManager")]
        [HttpPost("/hotel/delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await hotelService.Delete(id);
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }
    }
}

