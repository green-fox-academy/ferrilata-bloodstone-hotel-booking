using HotelBookingApp;
using HotelBookingApp.Models;
using HotelBookingApp.Services;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
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
        public void CheckUserByEmail_ReturnsTrue()
        {
            // Arrange
            var data = new List<UserModel>
            {
                new UserModel { Email = "testUser@example.com", Password = "12344321" }
            }.AsQueryable();
            var mockSet = new Mock<DbSet<UserModel>>();
            mockSet.As<IQueryable<UserModel>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<UserModel>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<UserModel>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<UserModel>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());
            var mockContext = new Mock<ApplicationContext>(GetTestDbOptions());
            mockContext.Setup(m => m.Users)
                    .Returns(mockSet.Object);
            var userService = new UserService(mockContext.Object);

            // Act & Assert
            Assert.True(userService.CheckUserByEmail("testUser@example.com"));
        }

        [Fact]
        public void checkUserByEmail_ReturnsFalse()
        {
            // Arrange
            var data = new List<UserModel>
            {
                new UserModel { Email = "testUser@example.com", Password = "12344321" }
            }.AsQueryable();
            var mockSet = new Mock<DbSet<UserModel>>();
            mockSet.As<IQueryable<UserModel>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<UserModel>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<UserModel>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<UserModel>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());
            var mockContext = new Mock<ApplicationContext>(GetTestDbOptions());
            mockContext.Setup(m => m.Users)
                    .Returns(mockSet.Object);
            var userService = new UserService(mockContext.Object);

            // Act & Assert
            Assert.False(userService.CheckUserByEmail("not_exists@example.com"));
        }
    }
}
