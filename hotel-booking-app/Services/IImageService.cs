﻿using HotelBookingApp.Models.Image;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HotelBookingApp.Services
{
    public interface IImageService
    {
        Task<List<ImageDetails>> GetImageListAsync(int hotelId);
        Task<List<IFormFile>> UploadImagesAsync(List<IFormFile> files, int hotelid);
        Task DeleteFileAsync(string fileName);
        Task DeleteAllFileAsync(int hotelId);
    }
}