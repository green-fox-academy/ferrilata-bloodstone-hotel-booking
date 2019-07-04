using HotelBookingApp.Models.Hotel;
using System.IO;
using System.Threading.Tasks;

namespace HotelBookingApp.Services
{
    public interface IThumbnailService
    {
        Task UploadAsync(Hotel hotel, Stream stream);
        Task SetBlobPermissionToPublic();
        Task DeleteAsync(int hotelId);
    }
}
