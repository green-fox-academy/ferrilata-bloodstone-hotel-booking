using HotelBookingApp.Data;
using HotelBookingApp.Exceptions;
using HotelBookingApp.Models.Hotel;
using HotelBookingApp.Utils;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelBookingApp.Services
{
    public class HotelService : IHotelService
    {
        private readonly ApplicationContext applicationContext;
        private readonly IImageService imageService;
        private readonly IThumbnailService thumbnailService;

        public HotelService(ApplicationContext applicationContext, IImageService imageService, IThumbnailService thumbnailService)
        {
            this.applicationContext = applicationContext;
            this.imageService = imageService;
            this.thumbnailService = thumbnailService;
        }

        public async Task Add(Hotel hotel)
        {
            await applicationContext.AddAsync(hotel);
            await applicationContext.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var hotel = applicationContext.Hotels
                .SingleOrDefault(h => h.HotelId == id)
                ?? throw new ItemNotFoundException($"Hotel with id: {id} is not found.");
            applicationContext.Hotels.Remove(hotel);
            await applicationContext.SaveChangesAsync();
            await thumbnailService.DeleteAsync(id);
            await imageService.DeleteAllFileAsync(id);
        }

        public async Task<IEnumerable<Hotel>> FindAll()
        {
            return await applicationContext.Hotels.ToListAsync();
        }

        public async Task<IEnumerable<Hotel>> FindAllOrderByName()
        {
            return await applicationContext.Hotels
                .OrderBy(h => h.Name)
                .ToListAsync();
        }

        public async Task<Hotel> FindByIdAsync(int id)
        {
            var hotel = await applicationContext.Hotels
                .Include(h => h.Location)
                .Include(h => h.PropertyType)
                .SingleOrDefaultAsync(h => h.HotelId == id)
                ?? throw new ItemNotFoundException($"Hotel with id: {id} is not found.");
            return hotel;
        }


        public async Task<PaginatedList<Hotel>> FindWithQuery(QueryParams queryParams)
        {
            var filteredHotels = applicationContext.Hotels
                .Include(h => h.Location)
                .Where(h =>
                    string.IsNullOrEmpty(queryParams.Search)
                    || h.Location.City.Contains(queryParams.Search));

            var orderedHotels = QueryableUtils<Hotel>.OrderCustom(filteredHotels, queryParams);
            return await PaginatedList<Hotel>.CreateAsync(orderedHotels, queryParams.CurrentPage, queryParams.PageSize);
        }
    }
}
