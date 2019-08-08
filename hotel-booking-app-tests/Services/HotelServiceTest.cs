using AutoMapper;
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
        private readonly Mock<IMapper> mapper;

        public HotelServiceTest()
        {
            options = TestDbOptions.Get();
            imageServiceMock = new Mock<IImageService>();
            thumbnailServiceMock = new Mock<IThumbnailService>();
            localizerMock = new Mock<IStringLocalizer<HotelService>>();
            mapper = new Mock<IMapper>();
        }

        [Fact]
        public async Task Add_WhenCalled_ShouldAddAHotel()
        {
            using (var context = new ApplicationContext(options))
            {
                var hotelService = new HotelService(context,
                    imageServiceMock.Object,
                    thumbnailServiceMock.Object,
                    localizerMock.Object,
                    mapper.Object);
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

        [Fact]
        public async Task Delete_WhenCalled_ShouldDeleteAHotel()
        {
            var id = 0;
            var lenght = 0;
            using (var context = new ApplicationContext(options))
            {
                lenght = context.Hotels.Count();
                var hotel = context.Add(new Hotel());
                await context.SaveChangesAsync();
                id = hotel.Entity.HotelId;
            }
            using (var context = new ApplicationContext(options))
            {
                var hotelService = new HotelService(context,
                    imageServiceMock.Object,
                    thumbnailServiceMock.Object,
                    localizerMock.Object,
                    mapper.Object);
                var contextLenght = context.Hotels.Count();
                var hotel = context.Hotels.SingleOrDefault(h => h.HotelId == id);
                Assert.Equal(lenght + 1, context.Hotels.Count());

                await hotelService.Delete(id);

                Assert.Equal(lenght, context.Hotels.Count());
                Assert.Equal(contextLenght - 1, context.Hotels.Count());
                Assert.False(context.Hotels.Contains(hotel));
            }
        }

        [Fact]
        public async Task Update_WhenCalled_ShouldUpdateAHotel()
        {
            var id = 0;
            var lenght = 0;
            using (var context = new ApplicationContext(options))
            {
                var pt = context.PropertyTypes.Add(new PropertyType());
                var ptId = pt.Entity.PropertyTypeId;

                lenght = context.Hotels.Count();

                var hotel = context.Add(
                    new Hotel {PropertyTypeId = ptId, StarRating = 5});
                await context.SaveChangesAsync();

                id = hotel.Entity.HotelId;
            }
            using (var context = new ApplicationContext(options))
            {
                var hotelService = new HotelService(context,
                    imageServiceMock.Object,
                    thumbnailServiceMock.Object,
                    localizerMock.Object,
                    mapper.Object);
                Assert.Equal(lenght + 1, context.Hotels.Count());
                var hotel = context.Hotels.SingleOrDefault(h => h.HotelId == id);
                hotel.Name = "not new";

                await hotelService.Update(hotel);

                Assert.Equal("not new", hotel.Name);
                Assert.True(hotel.StarRating == 5);
            }
        }

        [Fact]
        public async Task AddReview_WhenCalled_ShouldAddAReview()
        {
            using (var context = new ApplicationContext(options))
            {
                var hotelService = new HotelService(context,
                    imageServiceMock.Object,
                    thumbnailServiceMock.Object,
                    localizerMock.Object,
                    mapper.Object);
                var contextLength = context.Reviews.Count();
                var comment = "comment";

                var review = await hotelService.AddReviewAsync(
                    new Review {Comment = comment }
                );

                Assert.Equal(contextLength + 1, context.Reviews.Count());
                Assert.True(context.Reviews.Contains(review));
                Assert.Equal(comment, context.Reviews.Find(review.ReviewId).Comment);
            }
        }
    }
}