using HotelBookingApp.Models.Account;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelBookingApp.Services
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;

        public AccountService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        public async Task<List<string>> SignInAsync(LoginRequest request)
        {
            var errors = new List<string>();
            var result = await signInManager.PasswordSignInAsync(request.Email, request.Password, request.RememberMe, lockoutOnFailure: true);
            if (result.IsLockedOut)
            {
                errors.Add("User account locked out.");
            }
            if (result.IsNotAllowed)
            {
                errors.Add("User is not allowed to login.");
            }
            if (result.RequiresTwoFactor)
            {
                errors.Add("Two factor authentication is required.");
            }
            if (!result.Succeeded)
            {
                errors.Add("Invalid login attempt");
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
    }
}
