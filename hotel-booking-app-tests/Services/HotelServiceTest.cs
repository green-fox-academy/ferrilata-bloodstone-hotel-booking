using HotelBookingApp;
using HotelBookingApp.Models.Hotel;
using HotelBookingApp.Services;
using HotelBookingAppTests.TestUtils;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace HotelBookingAppTests.Services
{
    public class HotelServiceTest
    {

        [Fact]
        public async Task Add_AddNewHotel_Ok()
        {
            //Arrange
            var hotel = new HotelModel(1, "hotelname", "suchdescription", 1);
            var context = new ApplicationContext(TestDbOptions.GetTestDbOptions());
            var hotelService = new HotelService(context);

            //Act
            await hotelService.Add(hotel);
            var actual = context.Hotels.ToList().Count;

            //Assert
            var expect = 1;
            Assert.Equal(expect, actual);
        }
    }
}