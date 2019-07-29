using HotelBookingApp.Models.HotelModels;
using System.Threading.Tasks;

namespace HotelBookingApp.Services
{
    public interface IEmailService
    {
        Task SendEmailAsync(Reservation reservation, string userEmail);
    }
}
