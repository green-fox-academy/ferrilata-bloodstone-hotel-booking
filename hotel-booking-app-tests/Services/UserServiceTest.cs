using HotelBookingApp;
using HotelBookingApp.Models;
using HotelBookingApp.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace hotel_booking_app_tests.Services
{
    public class UserServiceTest
    {
        public static DbContextOptions<ApplicationContext> GetTestDbOptions()
        {
            return new DbContextOptionsBuilder<ApplicationContext>()
                .UseInMemoryDatabase(databaseName: "hotel-booking-testdb")
                .Options;
        }

        [Fact]
        public async Task Create_WithNewUser_ResultsOk()
        {
            // Arrange
            var newUser = new UserModel { Email = "testUser@example.com", Password = "12344321" };
            var context = new ApplicationContext(GetTestDbOptions());
            var userService = new UserService(context);
            var expected = 1;

            // Act
            await userService.Create(newUser);
            var actual = context.Users.ToList().Count;

            //Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task Create_WithExistingUser_ThrowsException()
        {
            using (var context = new ApplicationContext(GetTestDbOptions()))
            {
                var newUser = new UserModel { Email = "testUser@example.com", Password = "12344321" };
                context.Add(new UserModel { Email = "testUser2@example.com", Password = "12344321" });
                var userService = new UserService(context);

                // Act & Assert
                Exception ex = await Assert.ThrowsAsync<Exception>(() => userService.Create(newUser));
                Assert.Equal($"User with email {newUser.Email} already exists.", ex.Message);
            }
        }

        [Fact]
        public void checkUserByEmail_ReturnsFalse()
        {
            using (var context = new ApplicationContext(GetTestDbOptions()))
            {
                var newUser = new UserModel { Email = "testUser@example.com", Password = "12344321" };
                context.Add(new UserModel { Email = "testUser2@example.com", Password = "12344321" });
                var userService = new UserService(context);

                // Act & Assert
                Assert.False(userService.checkUserByEmail("testString"));
            }
        }
    }
}
