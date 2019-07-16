using HotelBookingApp.Models.EmailModels;
using HotelBookingApp.Models.HotelModels;
using SendGrid;
using System.Threading.Tasks;

namespace HotelBookingApp.Services
{
    public interface IEmailService
    {
        Task<Response> SendMailAsync(Reservation reservation, string userEmail);
    }
}
