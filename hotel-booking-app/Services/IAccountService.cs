using HotelBookingApp.Models.Account;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HotelBookingApp.Services
{
    public interface IAccountService
    {
        Task<List<string>> SignIn(LoginRequest request);
        Task SignOut();
        Task<List<string>> SignUp(SignupRequest request);
    }
}
