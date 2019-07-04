using HotelBookingApp.Models.Account;
using System.Threading.Tasks;

namespace HotelBookingApp.Services
{
    public interface IAccountService
    {
        Task<string[]> SignIn(LoginRequest request);
        Task SignOut();
        Task<string[]> SignUp(SignupRequest request);
    }
}
