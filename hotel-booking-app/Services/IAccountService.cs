using HotelBookingApp.Models.Account;
using HotelBookingApp.Models.API;
using HotelBookingApp.Pages;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HotelBookingApp.Services
{
    public interface IAccountService
    {
        Task<List<string>> SignInAsync(LoginRequest request);
        Task<LoginResponse> SignInApiAsync(LoginRequest request);
        Task SignOutAsync();
        Task<List<string>> SignUpAsync(SignupRequest request);
        AuthenticationProperties ConfigureExternalAuthenticationProperties(string provider, string redirectUrl);
        Task<ExternalLoginInfo> GetExternalLoginInfoAsync();
        Task<SignInResult> ExternalLoginSignInAsync(string loginProvider, string providerKey, bool isPersistent);
        Task<List<string>> CreateAndLoginGoogleUser(ExternalLoginInfo info);
        Task<List<string>> ChangePasswordAsync(SettingViewModel model);
        Task<ApplicationUser> FindByIdAsync(string userId);
    }
}
