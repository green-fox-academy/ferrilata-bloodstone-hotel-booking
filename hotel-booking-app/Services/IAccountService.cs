using HotelBookingApp.Models.Account;
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
        Task SignOutAsync();
        Task<List<string>> SignUpAsync(SignupRequest request);
        AuthenticationProperties ConfigureExternalAuthenticationProperties(string provider, string redirectUrl);
        Task<ExternalLoginInfo> GetExternalLoginInfoAsync();
        Task<SignInResult> ExternalLoginSignInAsync(string loginProvider, string providerKey, bool isPersistent);
        Task<List<string>> CreateAndLoginGoogleUser(ExternalLoginInfo info);
        Task ResetPasswordAsync(string email, string newPassword);
        string CreateRandomPassword(int length = 8);
        Task<List<string>> ChangePasswordAsync(SettingViewModel model);
        Task<ApplicationUser> FindByIdAsync(string userId);
    }
}
