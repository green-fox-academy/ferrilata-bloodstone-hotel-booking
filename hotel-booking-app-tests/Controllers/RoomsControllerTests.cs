using HotelBookingApp.Controllers;
using HotelBookingApp.Models.HotelModels;
using HotelBookingApp.Pages;
using HotelBookingApp.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace HotelBookingAppTests.Controllers
{
    public class RoomControllerTests
    {
        private readonly Mock<IRoomService> roomServiceMock;
        private readonly Mock<IBedService> bedServiceMock;
        private readonly Mock<IHotelService> hotelServiceMock;

        public RoomControllerTests()
        {
            roomServiceMock = new Mock<IRoomService>();
            bedServiceMock = new Mock<IBedService>();
            hotelServiceMock = new Mock<IHotelService>();
        }

        [Fact]
        public async Task Add_WhenValid_ShouldCallServiceAndRedirect()
        {
            var controller = new RoomsController(bedServiceMock.Object, roomServiceMock.Object, hotelServiceMock.Object);
            var hotelId = 1;
            var room = new Room();

            var result = await controller.Add(hotelId, room);
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            var resultHotelId = (int)redirectResult.RouteValues["id"];

            Assert.Equal("Hotels", redirectResult.ControllerName);
            Assert.Equal(hotelId, resultHotelId);
            Assert.Equal(nameof(HotelsController.Hotel), redirectResult.ActionName);
            roomServiceMock.Verify(s => s.AddRoom(hotelId, room), Times.Once);
        }

        [Fact]
        public async Task AddBed_WhenValid_ShouldCallServiceAndRedirect()
        {
            var controller = new RoomsController(bedServiceMock.Object, roomServiceMock.Object, hotelServiceMock.Object);
            var hotelId = 1;
            var bedViewModel = new BedViewModel();

            var result = await controller.AddBed(hotelId, bedViewModel);
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            var resultHotelId = (int)redirectResult.RouteValues["id"];

            Assert.Equal("Hotels", redirectResult.ControllerName);
            Assert.Equal(nameof(HotelsController.Hotel), redirectResult.ActionName);
            Assert.Equal(hotelId, resultHotelId);
            bedServiceMock.Verify(s => s.AddBed(bedViewModel), Times.Once);
        }
    }
}
