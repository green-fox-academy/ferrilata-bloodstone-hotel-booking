using HotelBookingApp.Models.HotelModels;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Threading.Tasks;

namespace HotelBookingApp.Services
{
    public interface IThumbnailService
    {
        Task UploadAsync(Hotel hotel, Stream stream);
        Task SetBlobPermissionToPublic();
        Task DeleteAsync(int hotelId);
        Task UpdateThumbnail(int hotelId, IFormFile file);
        Task UpdateThumbnailFromUrl(int hotelId, string url);
    }
}
