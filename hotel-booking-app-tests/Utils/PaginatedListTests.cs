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
        private IEnumerable<Model> source;

        [Fact]
        public async Task PaginatedList_WhenCreating_PropertiesShouldBeOk()
        {
            source = InitSource();
            var mockSet = MockProvider.GetQueriable(source);

            var paginatedList = await PaginatedList<Model>.CreateAsync(mockSet.Object, 1, 1);

            Assert.Equal(1, paginatedList.CurrentPage);
        }

        private IEnumerable<Model> InitSource()
        {
            return new List<Model> {
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
        }

        public class Model
        {
            public string Name { get; set; }
        }
    }
}
