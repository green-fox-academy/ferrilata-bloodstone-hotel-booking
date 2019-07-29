using HotelBookingApp.Data;
using HotelBookingApp.Models.HotelModels;
using HotelBookingApp.Utils;
using HotelBookingAppTests.TestUtils;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace HotelBookingAppTests.Utils
{
    [Collection("Database collection")]
    public class PaginatedListTests
    {
        private readonly DbContextOptions<ApplicationContext> options = TestDbOptions.Get();

        [Fact]
        public async Task PaginatedList_WhenCreating_SizeShuldBeOk()
        {
            using (var context = new ApplicationContext(options))
            {
                int currentPage = 1;
                int pageSize = 1;

                var paginatedList = await PaginatedList<Hotel>
                    .CreateAsync(context.Hotels, currentPage, pageSize);

                Assert.Equal(currentPage, paginatedList.CurrentPage);
                Assert.Equal(pageSize, paginatedList.PageSize);
                Assert.Equal(pageSize, paginatedList.Count);
            }
        }

        [Fact]
        public async Task PaginatedList_WhenCreatingWith12Item_TotalPagesShouldBe3()
        {
            using (var context = new ApplicationContext(options))
            {
                int currentPage = 1;
                int pageSize = 4;

                var paginatedList = await PaginatedList<Hotel>
                    .CreateAsync(context.Hotels.Take(12), currentPage, pageSize);

                Assert.Equal(3, paginatedList.TotalPages);
            }
        }

        [Fact]
        public async Task PaginatedList_WhenAccessingNonExistingPage_ShouldReturnEmpty()
        {
            using (var context = new ApplicationContext(options))
            {
                int currentPage = 100;

                var paginatedList = await PaginatedList<Hotel>
                    .CreateAsync(context.Hotels, currentPage);

                Assert.Empty(paginatedList);
            }
        }

        [Fact]
        public async Task PaginatedList_WhenCreating_ShouldRetainOrder()
        {
            using (var context = new ApplicationContext(options))
            {
                int currentPage = 1;

                var paginatedList = await PaginatedList<Hotel>
                    .CreateAsync(context.Hotels, currentPage);

                Assert.Equal(context.Hotels.First(), paginatedList[0]);
            }
        }
    }
}
