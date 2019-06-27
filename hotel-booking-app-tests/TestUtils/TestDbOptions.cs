﻿using HotelBookingApp;
using Microsoft.EntityFrameworkCore;

namespace HotelBookingAppTests.TestUtils
{
    class TestDbOptions
    {
        public static DbContextOptions<ApplicationContext> GetTestDbOptions()
        {
            return new DbContextOptionsBuilder<ApplicationContext>()
                .UseInMemoryDatabase(databaseName: "hotel-booking-testdb")
                .Options;
        }
    }
}