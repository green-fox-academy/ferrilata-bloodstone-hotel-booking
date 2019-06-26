using HotelBookingApp;
using HotelBookingApp.Models.User;
using HotelBookingApp.Services;
using HotelBookingAppTests.TestUtils;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace HotelBookingAppTests.Services
{
    public class UserServiceTest
    {
        [Fact]
        public async Task Exists_WithNewUser_ResultsOk()
        {
            // Arrange
            var user = new UserModel { Id = 1, Email = "testUser@example.com", Password = "12344321" };
            var userQueryableList = new List<UserModel>();
            var mockContext = new ApplicationContext(TestDbOptions.GetTestDbOptions());
            var userService = new UserService(mockContext);

            // Act
            await userService.Create(user);
            var actual = mockContext.Users.ToList().Count;

            //Assert
            var expect = 1;
            Assert.Equal(expect, actual);
        }

        [Fact]
        public async Task Exists_WithExistingUser_ThrowsException()
        {
            // Arrange
            var user = new UserModel { Id = 1, Email = "testUser@example.com", Password = "12344321" };
            var userQueryableList = new List<UserModel> { user };
            var mockSet = GetDbSetForAsynctest.GetMockDbSetForAsyncMethodTest<UserModel>(userQueryableList);

            var mockContext = new Mock<ApplicationContext>(TestDbOptions.GetTestDbOptions());
            mockContext.Setup(c => c.Users).Returns(mockSet.Object);
            var userService = new UserService(mockContext.Object);

            // Act & Assert
            Exception ex = await Assert.ThrowsAsync<Exception>(() => userService.Create(user));
            Assert.Equal($"User with email {user.Email} already exists.", ex.Message);
            
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
            var mockContext = new Mock<ApplicationContext>(TestDbOptions.GetTestDbOptions());
            mockContext.Setup(m => m.Users)
                    .Returns(mockSet.Object);
            var userService = new UserService(mockContext.Object);

            // Act & Assert
            Assert.True(userService.Exists("testUser@example.com"));
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
            var mockContext = new Mock<ApplicationContext>(TestDbOptions.GetTestDbOptions());
            mockContext.Setup(m => m.Users)
                    .Returns(mockSet.Object);
            var userService = new UserService(mockContext.Object);

            // Act & Assert
            Assert.False(userService.Exists("not_exists@example.com"));
        }

        [Fact]
        public async Task Delete_WithValidUserId_Ok()
        {
            // Arrange
            var user = new UserModel { Id = 1, Email = "testUser@example.com", Password = "12344321" };
            var userQueryableList = new List<UserModel> { user };
            var mockSet = GetDbSetForAsynctest.GetMockDbSetForAsyncMethodTest<UserModel>(userQueryableList);
            var mockContext = new Mock<ApplicationContext>(TestDbOptions.GetTestDbOptions());
            mockContext.Setup(c => c.Users).Returns(mockSet.Object);
            var userService = new UserService(mockContext.Object);

            // Act
            await userService.Delete(1);

            // Assert
            var userList = mockContext.Object.Users.ToList();

            var userListLength = mockContext.Object.Users.ToList().Count;
            Assert.Equal(0, userListLength);
            mockSet.Verify(m => m.Remove(It.IsAny<UserModel>()), Times.Once());
        }

        [Fact]
        public async Task GetById_WithValidID_ReturnsUser()
        {
            // Arrange
            var user = new UserModel { Id = 1, Email = "testUser@example.com", Password = "12344321" };
            var userQueryableList = new List<UserModel> { user };
            var mockSet = GetDbSetForAsynctest.GetMockDbSetForAsyncMethodTest<UserModel>(userQueryableList);
            var mockContext = new Mock<ApplicationContext>(TestDbOptions.GetTestDbOptions());
            mockContext.Setup(c => c.Users).Returns(mockSet.Object);
            mockContext.Setup(c => c.Users.FindAsync(It.IsAny<long>())).ReturnsAsync(user);
            var userService = new UserService(mockContext.Object);

            // Act
            var actual = await userService.GetById(1);

            // Assert
            Assert.Equal(user.Email, actual.Email);
        }

        [Fact]
        public async Task GetById_WithInValidID_TrhowsExceptionAsync()
        {
            // Arrange
            var user = new UserModel { Id = 1, Email = "testUser@example.com", Password = "12344321" };
            var userQueryableList = new List<UserModel> { user };
            var mockSet = GetDbSetForAsynctest.GetMockDbSetForAsyncMethodTest<UserModel>(userQueryableList);

            var mockContext = new Mock<ApplicationContext>(TestDbOptions.GetTestDbOptions());
            mockContext.Setup(c => c.Users).Returns(mockSet.Object);
            mockContext.Setup(c => c.FindAsync(It.IsAny<Type>())).ReturnsAsync(user);
            var userService = new UserService(mockContext.Object);

            // Act & Assert
            KeyNotFoundException ex = await Assert.ThrowsAsync<KeyNotFoundException>(() => userService.GetById(1));
            Assert.Equal("User with id: 1 is not found.", ex.Message);
        }

        [Fact]
        public async Task Update_WithValidUser_Ok()
        {
            // Arrange
            var user = new UserModel { Id = 1, Email = "testUser@example.com", Password = "12344321" };
            var userQueryableList = new List<UserModel> { user };
            var mockSet = GetDbSetForAsynctest.GetMockDbSetForAsyncMethodTest<UserModel>(userQueryableList);

            var mockContext = new Mock<ApplicationContext>(TestDbOptions.GetTestDbOptions());
            mockContext.Setup(c => c.Users).Returns(mockSet.Object);
            var userService = new UserService(mockContext.Object);
            var userModified = new UserModel { Id = 1, Email = "modified@example.com", Password = "12344321" };

            // Act
            await userService.Update(userModified);

            // Assert
            mockSet.Verify(m => m.Update(It.IsAny<UserModel>()), Times.Once(), "User updated more or less than 1.");
        }
    }
}
