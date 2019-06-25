using HotelBookingApp;
using Microsoft.EntityFrameworkCore;


namespace hotel_booking_app_tests.Services
{
    [TestClass]
    class UserServiceTest
    {
        public static DbContextOptions<ApplicationContext> GetTestDbOptions()
        {
            return new DbContextOptionsBuilder<ApplicationContext>()
                .UseInMemoryDatabase(databaseName: "hotel-booking-testdb")
                .Options;
        }