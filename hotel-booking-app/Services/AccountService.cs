using HotelBookingApp.Models.Account;
using HotelBookingApp.Pages;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace HotelBookingApp.Services
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly IStringLocalizer<AccountService> localizer;
        private readonly IEmailService emailService;

        public AccountService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IStringLocalizer<AccountService> localizer, IEmailService emailService)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.localizer = localizer;
            this.emailService = emailService;
        }

        public async Task<List<string>> SignInAsync(LoginRequest request)
        {
            var errors = new List<string>();
            var result = await signInManager.PasswordSignInAsync(request.Email, request.Password, request.RememberMe, lockoutOnFailure: true);
            if (result.IsLockedOut)
            {
                errors.Add(localizer["User account locked out."]);
            }
            if (result.IsNotAllowed)
            {
                errors.Add(localizer["User is not allowed to login."]);
            }
            if (result.RequiresTwoFactor)
            {
                errors.Add(localizer["Two factor authentication is required."]);
            }
            if (!result.Succeeded)
            {
                errors.Add(localizer["Invalid login attempt"]);
            }
            return errors;
        }

        public async Task SignOutAsync()
        {
            await signInManager.SignOutAsync();
        }

        public async Task<List<string>> SignUpAsync(SignupRequest request)
        {
            var user = new ApplicationUser { UserName = request.Email, Email = request.Email };
            var result = await userManager.CreateAsync(user, request.Password);

            if (result.Succeeded)
            {
                var role = request.IsManager ? "HotelManager" : "User";
                await userManager.AddToRoleAsync(user, role);
                await signInManager.SignInAsync(user, isPersistent: false);
            }

            return result.Errors
                .Select(e => e.Description)
                .ToList();
        }

        public AuthenticationProperties ConfigureExternalAuthenticationProperties(string provider, string redirectUrl)
        {
            return signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
        }

        public async Task<ExternalLoginInfo> GetExternalLoginInfoAsync()
        {
            return await signInManager.GetExternalLoginInfoAsync();
        }

        public async Task<SignInResult> ExternalLoginSignInAsync(string loginProvider, string providerKey, bool isPersistent)
        {
            return await signInManager.ExternalLoginSignInAsync(loginProvider, providerKey, isPersistent);
        }

        public async Task<List<string>> CreateAndLoginGoogleUser(ExternalLoginInfo info)
        {
            var user = new ApplicationUser
            {
                Email = info.Principal.FindFirst(ClaimTypes.Email).Value,
                UserName = info.Principal.FindFirst(ClaimTypes.Email).Value,
            };

            IdentityResult identResult = await userManager.CreateAsync(user);
            if (identResult.Succeeded)
            {
                await userManager.AddToRoleAsync(user, "User");
                identResult = await userManager.AddLoginAsync(user, info);
                if (identResult.Succeeded)
                {
                    await signInManager.SignInAsync(user, false);
                }
            }
            return identResult.Errors
                .Select(e => e.Description)
                .ToList();
        }

        public async Task<List<string>> ResetPasswordAsync(string email)
        {
            var errors = new List<string>();
            string newPassword = CreateRandomPassword(10);
            var user = await userManager.FindByEmailAsync(email);
            if (user != null)
            {
                var token = await userManager.GeneratePasswordResetTokenAsync(user);
                var result = await userManager.ResetPasswordAsync(user, token, newPassword);
                if (!result.Succeeded)
                {
                    errors.Add(localizer["Couldn't reset password"]);
                }
                await emailService.SendPasswordResetEmailAsync(newPassword, email);
            }
            else
            {
                errors.Add(localizer["Email doesn't exist"]);
            }
            return errors;
        }

        private string CreateRandomPassword(int length)
        {
            string validChars = "ABCDEFGHJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            string upper = "ABCDEFGHJKLMNOPQRSTUVWXYZ";
            string number = "0123456789";
            char[] chars = new char[length];
            Random random = new Random();
            chars[0] = upper[random.Next(0, upper.Length)];
            chars[1] = number[random.Next(0, number.Length)];
            for (int i = 2; i < length; i++)
            {
                chars[i] = validChars[random.Next(0, validChars.Length)];
            }
            return new string(chars);
        }


        public async Task<List<string>> ChangePasswordAsync(SettingViewModel model)
        {
            var errors = new List<string>();
            var result = await userManager.ChangePasswordAsync(model.ApplicationUser, model.Password, model.NewPassword);
            if (!result.Succeeded)
            {
                errors.Add(localizer["Invalid current password"]);
            }
            return errors;
        }

        public async Task<ApplicationUser> FindByIdAsync(string userId)
        {
            return await userManager.FindByIdAsync(userId);
        }
    }
}
