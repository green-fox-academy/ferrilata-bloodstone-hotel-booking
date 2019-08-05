using HotelBookingApp.Models.Account;
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

        public async Task ResetPasswordAsync(string email, string newPassword)
        {
            var user = await userManager.FindByEmailAsync(email);

            var token = await userManager.GeneratePasswordResetTokenAsync(user);
            await userManager.ResetPasswordAsync(user, token, newPassword);
            await emailService.SendPasswordResetEmailAsync(newPassword, email);
        }

        public string CreateRandomPassword(int length = 8)
        {  
            string validChars = "ABCDEFGHJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            Random random = new Random();
            
            char[] chars = new char[length];
            for (int i = 0; i < length; i++)
            {
                chars[i] = validChars[random.Next(0, validChars.Length)];
            }
            return new string(chars);
        }
    }
}
