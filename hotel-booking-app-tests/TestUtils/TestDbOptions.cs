using HotelBookingApp.Data;
using Microsoft.EntityFrameworkCore;

namespace HotelBookingAppTests.TestUtils
{
    class TestDbOptions
    {
        public static DbContextOptions<ApplicationContext> Get()
        {
            return new DbContextOptionsBuilder<ApplicationContext>()
                .UseInMemoryDatabase(databaseName: "hotel-booking-testdb")
                .Options;
        }
    }
}
