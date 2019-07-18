using HotelBookingApp.Utils;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace HotelBookingAppTests.Utils
{
    public class QueryableUtilsTests
    {
        [Fact]
        public void OrderCustom_ShouldOrderByAsc_WithString()
        {
            var source = GetSource();
            var queryParams = new QueryParams { OrderBy = "Name", Desc = false };

            var ordered = QueryableUtils<Model>.OrderCustom(source, queryParams);

            Assert.Equal(5, ordered.Count());
            Assert.Equal("A", ordered.First().Name);
            Assert.Equal("E", ordered.Last().Name);
        }

        [Fact]
        public void OrderCustom_ShouldOrderByDesc_WithString()
        {
            var source = GetSource();
            var queryParams = new QueryParams { OrderBy = "Name", Desc = true };

            var ordered = QueryableUtils<Model>.OrderCustom(source, queryParams);

            Assert.Equal(5, ordered.Count());
            Assert.Equal("E", ordered.First().Name);
            Assert.Equal("A", ordered.Last().Name);
        }

        [Fact]
        public void OrderCustom_ShouldOrderByAsc_WithInt()
        {
            var source = GetSource();
            var queryParams = new QueryParams { OrderBy = "Value", Desc = false };

            var ordered = QueryableUtils<Model>.OrderCustom(source, queryParams);

            Assert.Equal(5, ordered.Count());
            Assert.Equal(1, ordered.First().Value);
            Assert.Equal(5, ordered.Last().Value);
        }

        [Fact]
        public void OrderCustom_ShouldOrderByDesc_WithInt()
        {
            var source = GetSource();
            var queryParams = new QueryParams { OrderBy = "Value", Desc = true };

            var ordered = QueryableUtils<Model>.OrderCustom(source, queryParams);

            Assert.Equal(5, ordered.Count());
            Assert.Equal(5, ordered.First().Value);
            Assert.Equal(1, ordered.Last().Value);
        }

        [Fact]
        public void OrderCustom_ShouldRetainOrder_WithWrongParams()
        {
            var source = GetSource();
            var queryParams = new QueryParams { OrderBy = "NotExisting" };

            var ordered = QueryableUtils<Model>.OrderCustom(source, queryParams);

            Assert.Equal(source, ordered);
        }

        private IQueryable<Model> GetSource()
        {
            return new List<Model> {
                new Model { Name = "C", Value = 5 },
                new Model { Name = "E", Value = 3 },
                new Model { Name = "A", Value = 4 },
                new Model { Name = "B", Value = 1 },
                new Model { Name = "D", Value = 2 }
            }.AsQueryable();
        }

        private class Model
        {
            public string Name { get; set; }
            public int Value { get; set; }
        }
    }
}
