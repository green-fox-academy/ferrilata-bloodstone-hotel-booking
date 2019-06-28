using HotelBookingApp.Utils;
using HotelBookingAppTests.TestUtils;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace HotelBookingAppTests.Utils
{
    public class PaginatedListTests
    {
        private readonly IQueryable<Model> source;

        public PaginatedListTests()
        {
            source = InitSource();
        }

        [Fact]
        public async Task PaginatedList_WhenCreating_SizeShuldBeOk()
        {
            int currentPage = 1;
            int pageSize = 1;
         
            var paginatedList = await PaginatedList<Model>
                .CreateAsync(source, currentPage, pageSize);

            Assert.Equal(currentPage, paginatedList.CurrentPage);
            Assert.Equal(pageSize, paginatedList.PageSize);
            Assert.Equal(pageSize, paginatedList.Count);
        }

        [Fact]
        public async Task PaginatedList_WhenCreatingWith12Item_TotalPagesShouldBe3()
        {
            int currentPage = 1;
            int pageSize = 4;

            var paginatedList = await PaginatedList<Model>
                .CreateAsync(source, currentPage, pageSize);

            Assert.Equal(3, paginatedList.TotalPages);
        }

        [Fact]
        public async Task PaginatedList_WhenAccessingNonExistingPage_ShouldReturnEmpty()
        {
            int currentPage = 10;

            var paginatedList = await PaginatedList<Model>
                .CreateAsync(source, currentPage);

            Assert.Empty(paginatedList);
        }

        private IQueryable<Model> InitSource()
        {
            var dataList = new List<Model> {
                new Model { Name = "Krisz" },
                new Model { Name = "Geri" },
                new Model { Name = "Orsi" },
                new Model { Name = "Kata" },
                new Model { Name = "Alex" },
                new Model { Name = "Attila" },
                new Model { Name = "Andras" },
                new Model { Name = "Dani" },
                new Model { Name = "Zsolt" },
                new Model { Name = "Soma" },
                new Model { Name = "Roza" },
                new Model { Name = "Anna" },
            };
            var mockSet = MockProvider.GetQueriable(dataList);
            return mockSet.Object;
        }

        public class Model
        {
            public string Name { get; set; }
        }
    }
}
