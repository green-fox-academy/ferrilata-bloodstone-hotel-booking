using HotelBookingApp.Models.Account;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace HotelBookingApp.Data
{
    public static class ApplicationDbInitializer
    {
        public static void SeedData(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            SeedRoles(roleManager);
            SeedUsers(userManager);
        }

        private static void SeedRoles(RoleManager<IdentityRole> roleManager)
        {
            var roleNames = new List<string>() { "Admin", "HotelManager", "User" };
            foreach (var name in roleNames)
            {
                if (!roleManager.RoleExistsAsync(name).Result)
                {
                    var roleResult = roleManager.CreateAsync(new IdentityRole() { Name = name }).Result;
                }
            }
        }

        private static void SeedUsers(UserManager<ApplicationUser> userManager)
        {
            var password = "Passw0rd";
            var users = new Dictionary<string, string>
            {
                ["admin@bloodstone.com"] = "Admin",
                ["manager@bloodstone.com"] = "HotelManager",
                ["user@bloodstone.com"] = "User"
            };
            foreach (var userEntry in users)
            {
                if (userManager.FindByEmailAsync(userEntry.Key).Result == null)
                {
                    var user = new ApplicationUser
                    {
                        UserName = userEntry.Key,
                        Email = userEntry.Key
                    };
                    var result = userManager.CreateAsync(user, password).Result;
                    if (result.Succeeded)
                    {
                        userManager.AddToRoleAsync(user, userEntry.Value).Wait();
                    }
                }
            }
        }
    }
}
