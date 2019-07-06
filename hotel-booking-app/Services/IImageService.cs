using HotelBookingApp.Models.Image;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace HotelBookingApp.Services
{
    public interface IImageService
    {
        Task UploadAsync(string imageName, Stream stream);
        Task<List<ImageDetails>> GetImageListAsync(int hotelId);
        Task<List<IFormFile>> UploadImagesAsync(List<IFormFile> files, int hotelid);
        Task DeleteFileAsync(string fileName);
        Task DeleteAllFileAsync(int hotelId);
    }
}