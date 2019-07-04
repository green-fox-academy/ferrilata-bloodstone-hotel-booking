using HotelBookingApp.Models.Account;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HotelBookingApp.Services
{
    public interface IAccountService
    {
        Task<List<string>> SignInAsync(LoginRequest request);
        Task SignOutAsync();
        Task<List<string>> SignUpAsync(SignupRequest request);
    }
}
