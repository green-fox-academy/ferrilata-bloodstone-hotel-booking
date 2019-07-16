using HotelBookingApp.Data;
using HotelBookingApp.Models.HotelModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace HotelBookingAppTests.TestUtils
{
    public class DatabaseFixture : IDisposable
    {
        private readonly DbContextOptions<ApplicationContext> options;

        public DatabaseFixture()
        {
            options = TestDbOptions.Get();
            using (var context = new ApplicationContext(options))
            {
                SeedHotels(context);
                context.SaveChanges();
            }
        }

        public void Dispose()
        {
            using (var context = new ApplicationContext(options))
            {
                context.Hotels.RemoveRange(context.Hotels);
                context.SaveChanges();
            }
        }

        private void SeedHotels(ApplicationContext context)
        {
            context.Hotels.AddRange(new List<Hotel>
            {
                new Hotel { Name = "Hotel1" },
                new Hotel { Name = "Hotel2" },
                new Hotel { Name = "Hotel3" },
                new Hotel { Name = "Hotel4" },
                new Hotel { Name = "Hotel5" },
                new Hotel { Name = "Hotel6" },
                new Hotel { Name = "Hotel7" },
                new Hotel { Name = "Hotel8" },
                new Hotel { Name = "Hotel9" },
                new Hotel { Name = "Hotel10" },
                new Hotel { Name = "Hotel11" },
                new Hotel { Name = "Hotel12" },
                new Hotel { Name = "Hotel13" },
                new Hotel { Name = "Hotel14" },
                new Hotel { Name = "Hotel15" }
            });
        }
    }
}
