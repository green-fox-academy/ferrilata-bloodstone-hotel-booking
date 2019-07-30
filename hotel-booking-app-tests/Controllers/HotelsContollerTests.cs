using HotelBookingApp.Controllers;
using HotelBookingApp.Models.HotelModels;
using HotelBookingApp.Pages;
using HotelBookingApp.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Xunit;

namespace HotelBookingAppTests.Controllers
{
    public class HotelControllerTests
    {
        private readonly Mock<IHotelService> hotelServiceMock;
        private readonly Mock<IImageService> imageServiceMock;
        private readonly Mock<IThumbnailService> thumbnailServiceMock;
        private readonly Mock<IPropertyTypeService> propertyServiceMock;

        public HotelControllerTests()
        {
            hotelServiceMock = new Mock<IHotelService>();
            imageServiceMock = new Mock<IImageService>();
            thumbnailServiceMock = new Mock<IThumbnailService>();
            propertyServiceMock = new Mock<IPropertyTypeService>();
        }

        [Fact]
        public async Task AddReview_WhenValid_ShouldCallServiceAndRedirect()
        {
            var controllerContext = GetDefaultControllerContext();
            var controller = new HotelsController(hotelServiceMock.Object,
                imageServiceMock.Object, thumbnailServiceMock.Object, propertyServiceMock.Object)
            {
                ControllerContext = controllerContext
            };
            var hotelId = 1;
            var review = new Review();

            var result = await controller.AddReview(hotelId, review);
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            int resultHotelId = (int)redirectResult.RouteValues["id"];

            Assert.Null(redirectResult.ControllerName);
            Assert.Equal(nameof(controller.Hotel), redirectResult.ActionName);
            Assert.Equal(hotelId, resultHotelId);
            Assert.Equal(controllerContext.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier), review.ApplicationUserId);
            hotelServiceMock.Verify(s => s.AddReviewAsync(review), Times.Once);
        }

        private ControllerContext GetDefaultControllerContext()
        {
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, "User Name"),
                new Claim(ClaimTypes.NameIdentifier, "1")
            }, "mock"));

            return new ControllerContext
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };
        }
    }
}
