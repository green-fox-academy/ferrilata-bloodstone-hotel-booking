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

        public async Task<List<string>> SignIn(LoginRequest request)
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

        public async Task SignOut()
        {
            await signInManager.SignOutAsync();
        }

        public Task<List<string>> SignUp(SignupRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
