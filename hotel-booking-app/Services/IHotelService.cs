using HotelBookingApp.Models.HotelModels;
using HotelBookingApp.Models.API;
using HotelBookingApp.Utils;
using System.Threading.Tasks;

namespace HotelBookingApp.Services
{
    public interface IHotelService
    {
        Task<PaginatedList<Hotel>> FindWithQuery(QueryParams queryParams);
        Task<PaginatedList<Hotel>> FindWithQuery(QueryParams queryParams, string userId);
        Task<Hotel> Add(Hotel hotel);
        Task Delete(int hotelId);
        Task<Hotel> FindByIdAsync(int id);
        Task<Hotel> FindByIdAsync(int id, QueryParams queryParams);
        Task<Hotel> Update(Hotel hotel);
        Task<Review> AddReviewAsync(Review review);
        Task DeleteReview(int reviewId);
        HotelsDTO GetHotelDTOs(PaginatedList<Hotel> paginatedHotels);
        Task<PaginatedList<Review>> FindAllReviews(int hotelId, QueryParams queryParams);
    }
}
