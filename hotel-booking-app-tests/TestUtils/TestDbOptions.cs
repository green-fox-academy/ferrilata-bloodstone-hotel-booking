using HotelBookingApp;
using Microsoft.EntityFrameworkCore;

namespace hotel_booking_app_tests.TestUtils
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
