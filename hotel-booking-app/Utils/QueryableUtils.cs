using System.Linq;
namespace HotelBookingApp.Utils
{
    public static class QueryableUtils<T>
    {
        public static IQueryable<T> OrderCustom(IQueryable<T> source, QueryParams queryParams)
        {
            var prop = typeof(T).GetProperty(queryParams.OrderBy);
            if (prop == null)
            {
                return source;
            }

            return queryParams.Desc
                ? source.OrderByDescending(h => prop.GetValue(h))
                : source.OrderBy(h => prop.GetValue(h));
        }
    }
}
