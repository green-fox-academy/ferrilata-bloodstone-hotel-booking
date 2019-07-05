using HotelBookingApp.Exceptions;
using HotelBookingApp.Models.Hotel;
using HotelBookingApp.Pages;
using HotelBookingApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HotelBookingApp.Controllers
{
    [Authorize]
    public class HotelsController : Controller
    {
        private readonly IHotelService hotelService;
        private readonly IImageService imageService;

        public HotelsController(IHotelService hotelService, IImageService imageService)
        {
            this.hotelService = hotelService;
            this.imageService = imageService;
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
            var hotel = await hotelService.FindByIdAsync(id);
            var images = await imageService.GetImageListAsync(id);
            var model = new HotelViewModel
            {
                Hotel = hotel,
                ImageList = images
            };
            return View(model);
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
    }
}
