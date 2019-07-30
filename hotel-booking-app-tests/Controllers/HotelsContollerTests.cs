using HotelBookingApp.Controllers;
using HotelBookingApp.Models.HotelModels;
using HotelBookingApp.Services;
using HotelBookingAppTests.TestUtils;
using Microsoft.AspNetCore.Mvc;
using Moq;
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
            var controllerContext = ControllerContextProvider.GetDefault();
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

        [Fact]
        public async Task AddReview_WhenInvalid_ShouldReturnView()
        {
            var controller = new HotelsController(hotelServiceMock.Object,
                imageServiceMock.Object, thumbnailServiceMock.Object, propertyServiceMock.Object);
            controller.ModelState.AddModelError("Rating", "Required");
            var hotelId = 1;
            var review = new Review();

            var result = await controller.AddReview(hotelId, review);
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public async Task DeleteReview_ShouldCallServiceAndRedirect()
        {
            var controller = new HotelsController(hotelServiceMock.Object,
                imageServiceMock.Object, thumbnailServiceMock.Object, propertyServiceMock.Object);
            var hotelId = 1;
            var reviewId = 1;

            var result = await controller.DeleteReview(hotelId, reviewId);
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            int resultHotelId = (int)redirectResult.RouteValues["id"];

            Assert.Equal(hotelId, resultHotelId);
            Assert.Null(redirectResult.ControllerName);
            Assert.Equal(nameof(controller.Hotel), redirectResult.ActionName);
            hotelServiceMock.Verify(s => s.DeleteReview(reviewId), Times.Once);
        }
    }
}
