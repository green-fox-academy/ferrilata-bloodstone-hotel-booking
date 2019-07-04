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

        public Task<string[]> SignIn(LoginRequest request)
        {
            throw new NotImplementedException();
        }

        public async Task SignOut()
        {
            await signInManager.SignOutAsync();
        }

        public Task<string[]> SignUp(SignupRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
