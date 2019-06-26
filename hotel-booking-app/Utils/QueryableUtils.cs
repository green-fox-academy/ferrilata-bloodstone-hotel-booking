using System.Linq;
namespace HotelBookingApp.Utils
{
    public static class QueryableUtils<T>
    {
        public static IQueryable<T> OrderCustom(IQueryable<T> source, string orderBy, bool desc)
        {
            var prop = typeof(T).GetProperty(orderBy);
            if (prop == null)
            {
                return source;
            }

            return desc
                ? source.OrderByDescending(h => prop.GetValue(h))
                : source.OrderBy(h => prop.GetValue(h));
        }
    }
}
