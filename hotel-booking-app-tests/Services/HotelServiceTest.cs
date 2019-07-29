using HotelBookingApp.Data;
using HotelBookingApp.Models.HotelModels;
using HotelBookingApp.Services;
using HotelBookingAppTests.TestUtils;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Moq;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace HotelBookingAppTests.Services
{
    [Collection("Database collection")]
    public class HotelServiceTest
    {
        private readonly DbContextOptions<ApplicationContext> options;
        private readonly Mock<IImageService> imageServiceMock;
        private readonly Mock<IThumbnailService> thumbnailServiceMock;
        private readonly Mock<IStringLocalizer<HotelService>> localizerMock;

        public HotelServiceTest()
        {
            options = TestDbOptions.Get();
            imageServiceMock = new Mock<IImageService>();
            thumbnailServiceMock = new Mock<IThumbnailService>();
            localizerMock = new Mock<IStringLocalizer<HotelService>>();
        }

        [Fact]
        public async Task Add_WhenCalled_ShouldAddAHotel()
        {
            using (var context = new ApplicationContext(options))
            {
                var hotelService = new HotelService(context,
                    imageServiceMock.Object,
                    thumbnailServiceMock.Object,
                    localizerMock.Object);
                var contextLength = context.Hotels.Count();
                var hotelName = "New Hotel";

                var hotel = await hotelService.Add(
                    new Hotel { Name = hotelName }
                );

                Assert.Equal(contextLength + 1, context.Hotels.Count());
                Assert.True(context.Hotels.Contains(hotel));
                Assert.Equal(hotelName, context.Hotels.Find(hotel.HotelId).Name);
            }
        }
    }
}